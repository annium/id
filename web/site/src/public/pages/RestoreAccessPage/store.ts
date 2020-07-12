import { createState, resultToStatus, State } from '@annium/forms'
import { AsyncState } from '@annium/utils/dist/async'
import { INotificationStore } from '@annium/utils/dist/stores'
import { action, observable } from 'mobx'
import { lazyInject } from 'public/config/di/container'
import { services } from 'public/config/di/services'
import { meService } from 'shared/api/server/meService'


export type Data = { email: string }

export class Store {
  @observable
  public form: State<Data> = createState<Data>({ email: '' })
  @observable
  public state: AsyncState = new AsyncState()
  @observable
  public data: Data = { email: '' }
  @lazyInject(services.NotificationStore)
  public notifications!: INotificationStore

  public constructor() {
    delete this.notifications
  }

  @action.bound
  public async restoreAccess() {
    this.state.start()

    const result = await meService.restoreAccess(this.form.email.value)

    if (result.isFailure) {
      this.state.failure(result)

      this.form.setStatus(resultToStatus(result))
      if (result.plainErrors.length)
        this.notifications.error(result.plainErrors.join(', '))
      else if (result.labeledErrors.user)
        this.notifications.error(result.labeledErrors.user.join(', '))

      return
    }

    this.data = observable(this.form.value)
    this.state.success()
    this.form.reset()
  }
}
