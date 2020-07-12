import { createState, resultToStatus, State } from '@annium/forms'
import { AsyncState } from '@annium/utils/dist/async'
import { INotificationStore } from '@annium/utils/dist/stores'
import { action, observable } from 'mobx'
import { lazyInject } from 'public/config/di/container'
import { services } from 'public/config/di/services'
import { loginService } from 'shared/api/server/loginService'
import { IMeStore } from 'shared/stores/MeStore'
import { IRouterStore } from 'shared/stores/RouterStore'


export type Data = { login: string; password: string }

export class Store {
  @observable
  public form: State<Data> = createState<Data>({ login: '', password: '' })
  @observable
  public state: AsyncState = new AsyncState()
  @lazyInject(services.MeStore)
  public me!: IMeStore
  @lazyInject(services.RouterStore)
  public router!: IRouterStore
  @lazyInject(services.NotificationStore)
  public notifications!: INotificationStore

  public constructor() {
    delete this.me
    delete this.router
    delete this.notifications
  }

  @action.bound
  public async login() {
    this.state.start()

    const result = await loginService.login(this.form.login.value, this.form.password.value)

    if (result.isFailure) {
      this.state.failure(result)
      this.me.state.reset()

      this.form.setStatus(resultToStatus(result))
      if (result.plainErrors.length)
        this.notifications.error(result.plainErrors.join(', '))
      else if (result.labeledErrors.user)
        this.notifications.error(result.labeledErrors.user.join(', '))

      return
    }

    await this.me.load()
    if (this.me.hasAccess) {
      this.state.success()
      this.form.reset()
      this.router.goToHomeOrStartup()
    } else
      this.state.failure(this.me.state)
  }
}
