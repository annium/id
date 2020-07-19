import { createState, resultToStatus, State } from '@annium/forms'
import { AsyncState } from '@annium/utils/dist/async'
import { mapResponse } from '@annium/utils/dist/helpers'
import { INotificationStore } from '@annium/utils/dist/stores'
import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { App, AppResponseSchema } from 'member/models/App'
import { routes } from 'member/routes'
import { action, observable, toJS } from 'mobx'
import { appService } from 'shared/api/server/appService'
import { IRouterStore } from 'shared/stores/RouterStore'


export class Store {
  @observable
  public form: State<App> = createState<App>({
    id: '',
    name: '',
  })
  @observable
  public state: AsyncState = new AsyncState()
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
    this.state.start()

    const id = this.router.parse<{ app: string }>(routes.apps.edit).app
    const result = await appService.getApp(id).then(mapResponse(AppResponseSchema))

    if (result.isSuccess) {
      this.form.setValue(result.data)
      this.state.success()
    } else
      this.state.failure(result)
  }

  @action.bound
  public async update() {
    this.state.start()

    const result = await appService.updateApp(this.form.id.value, toJS(this.form.value))

    if (result.isSuccess) {
      this.state.success()
      this.form.reset()

      this.router.go(routes.apps.my)

      return
    }

    this.state.failure(result)

    this.form.setStatus(resultToStatus(result))
    if (result.plainErrors.length)
      this.notifications.error(result.plainErrors.join(', '))
    else if (result.labeledErrors.user)
      this.notifications.error(result.labeledErrors.user.join(', '))
  }
}
