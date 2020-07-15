import { createState, resultToStatus, State } from '@annium/forms'
import { AsyncDataState, AsyncState } from '@annium/utils/dist/async'
import { mapResponseArray } from '@annium/utils/dist/helpers'
import { INotificationStore } from '@annium/utils/dist/stores'
import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { App, AppResponseSchema } from 'member/models/App'
import { routes } from 'member/routes'
import { getLog } from 'member/utils/log'
import { action, IObservableValue, observable, runInAction, toJS } from 'mobx'
import { appService } from 'shared/api/server/appService'
import { IRouterStore } from 'shared/stores/RouterStore'


const log = getLog('AccountsStore')

export class Store {
  @observable
  public apps: AsyncDataState<App[]> = new AsyncDataState<App[]>([])
  @observable
  public myApps: AsyncDataState<App[]> = new AsyncDataState<App[]>([])
  @observable
  public form: State<App> = createState<App>({
    id: '',
    name: '',
  })
  @observable
  public state: AsyncState = new AsyncState()
  @observable
  public deleteCandidate: IObservableValue<string | null> = observable.box(null)
  @lazyInject(services.RouterStore)
  public router!: IRouterStore
  @lazyInject(services.NotificationStore)
  public notifications!: INotificationStore

  public constructor() {
    delete this.notifications
    delete this.router
  }

  @action.bound
  public async load() {
    this.apps.start()
    log('load')

    const result = await appService.list().then(mapResponseArray(AppResponseSchema))

    if (result.isSuccess)
      this.apps.success(result.data)
    else
      this.apps.failure(result)
  }

  @action.bound
  public async loadMy() {
    this.myApps.start()
    log('load')

    const result = await appService.listMy().then(mapResponseArray(AppResponseSchema))

    if (result.isSuccess)
      this.myApps.success(result.data)
    else
      this.myApps.failure(result)
  }

  @action.bound
  public async create() {
    this.state.start()

    const result = await appService.create(toJS(this.form.value))

    if (result.isSuccess) {
      this.state.success()
      this.form.reset()

      this.router.go(routes.apps.list)

      return
    }

    this.state.failure(result)

    this.form.setStatus(resultToStatus(result))
    if (result.plainErrors.length)
      this.notifications.error(result.plainErrors.join(', '))
    else if (result.labeledErrors.user)
      this.notifications.error(result.labeledErrors.user.join(', '))
  }


  @action.bound
  public async edit() {
    const id = this.router.get<{ id: string }>(routes.apps.update).id

    const app = this.myApps.data.find(x => x.id === id)
    if (!app)
      return

    this.form.setValue(app)
  }

  @action.bound
  public async update() {
    this.state.start()

    const result = await appService.update(this.form.id.value, toJS(this.form.value))

    if (result.isSuccess) {
      this.state.success()
      this.form.reset()

      await this.load()

      this.router.go(routes.apps.list)

      return
    }

    this.state.failure(result)

    this.form.setStatus(resultToStatus(result))
    if (result.plainErrors.length)
      this.notifications.error(result.plainErrors.join(', '))
    else if (result.labeledErrors.user)
      this.notifications.error(result.labeledErrors.user.join(', '))
  }

  @action.bound
  public setDeleteCandidate(candidate: string | null) {
    return () => runInAction(() => {
      this.deleteCandidate.set(candidate)
    })
  }

  @action.bound
  public async delete() {
    if (!this.deleteCandidate) {
      log('No delete candidate')

      return
    }

    this.apps.start()

    const id = this.deleteCandidate.get()!
    this.deleteCandidate.set(null)

    const result = await appService.delete(id)

    if (result.isFailure) {
      this.apps.success(this.apps.data)

      if (result.plainErrors.length)
        this.notifications.error(result.plainErrors.join(', '))

      return
    }

    await Promise.all([this.load(), this.loadMy()])
  }
}
