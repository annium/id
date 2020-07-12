import { createState, resultToStatus, State } from '@annium/forms'
import { AsyncDataState, AsyncState } from '@annium/utils/dist/async'
import { mapResponse, mapResponseArray } from '@annium/utils/dist/helpers'
import { INotificationStore } from '@annium/utils/dist/stores'
import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { Account, AccountResponseSchema } from 'member/models/Account'
import { routes } from 'member/routes'
import { getLog } from 'member/utils/log'
import { action, IObservableValue, observable, runInAction, toJS } from 'mobx'
// import { accountService } from 'shared/api/server/accountService'
// import { Exchange } from 'shared/api/server/client/shared'
import { IRouterStore } from 'shared/stores/RouterStore'


const log = getLog('AccountsStore')

export class Store {
  @observable
  public accounts: AsyncDataState<Account[]> = new AsyncDataState<Account[]>([])
  @observable
  public form: State<Account> = createState<Account>({
    id: '',
    name: '',
    exchange: Exchange.BitMEX,
    isTest: true,
    key: '',
    secret: '',
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
    this.accounts.start()

    const result = await accountService.listMyAccounts().then(mapResponseArray(AccountResponseSchema))

    if (result.isSuccess)
      this.accounts.success(result.data)
    else
      this.accounts.failure(result)
  }

  @action.bound
  public async create() {
    this.state.start()

    const result = await accountService.createAccount(toJS(this.form.value))

    if (result.isSuccess) {
      this.state.success()
      this.form.reset()

      await this.load()

      this.router.go(routes.accounts.list)

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
    const id = this.router.get<{ id: string }>(routes.accounts.update).id

    this.state.start()

    const result = await accountService.getMyAccount(id).then(mapResponse(AccountResponseSchema))

    if (result.isSuccess) {
      this.state.success()
      this.form.setValue(result.data)

      return
    }

    this.state.failure(result)

    if (result.plainErrors.length)
      this.notifications.error(result.plainErrors.join(', '))
  }

  @action.bound
  public async update() {
    this.state.start()

    const result = await accountService.updateAccount(this.form.id.value, toJS(this.form.value))

    if (result.isSuccess) {
      this.state.success()
      this.form.reset()

      await this.load()

      this.router.go(routes.accounts.list)

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

    this.accounts.start()

    const id = this.deleteCandidate.get()!
    this.deleteCandidate.set(null)

    const result = await accountService.deleteAccount(id)

    if (result.isFailure) {
      this.accounts.success(this.accounts.data)

      if (result.plainErrors.length)
        this.notifications.error(result.plainErrors.join(', '))

      return
    }

    await this.load()
  }
}
