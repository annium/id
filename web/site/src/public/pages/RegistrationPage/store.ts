import { createState, resultToStatus, State } from '@annium/forms'
import { AsyncState } from '@annium/utils/dist/async'
import { INotificationStore } from '@annium/utils/dist/stores'
import { action, observable } from 'mobx'
import { lazyInject } from 'public/config/di/container'
import { services } from 'public/config/di/services'
import { meService } from 'shared/api/server/meService'


export type Data = {
  email: string
  login: string
  referralId: string
}

export class Store {
  @observable
  public form: State<Data> = createState<Data>({ email: '', login: '', referralId: '' })
  @observable
  public state: AsyncState = new AsyncState()
  @lazyInject(services.NotificationStore)
  public notifications!: INotificationStore

  public constructor() {
    delete this.notifications
  }

  @action.bound
  public async register() {
    this.state.start()

    const result = await meService.registerMe(
      this.form.email.value,
      this.form.login.value,
      this.form.referralId.value || null,
    )

    if (result.isFailure) {
      this.state.failure(result)

      this.form.setStatus(resultToStatus(result))
      if (result.plainErrors.length)
        this.notifications.error(result.plainErrors.join(', '))

      return
    }

    this.state.success()
    this.form.reset()
  }
}
