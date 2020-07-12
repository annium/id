import { createState, resultToStatus, State } from '@annium/forms'
import { AsyncState } from '@annium/utils/dist/async'
import { INotificationStore } from '@annium/utils/dist/stores'
import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { action, observable, toJS } from 'mobx'
import { meService } from 'shared/api/server/meService'
import { IMeStore } from 'shared/stores/MeStore'


export type Data = { password: string }

export class Store {
  @observable
  public form: State<Data> = createState<Data>({
    password: '',
  })
  @observable
  public state: AsyncState = new AsyncState()
  @lazyInject(services.MeStore)
  public me!: IMeStore
  @lazyInject(services.NotificationStore)
  public notifications!: INotificationStore

  public constructor() {
    delete this.me
    delete this.notifications
  }

  @action.bound
  public async save() {
    this.state.start()

    const result = await meService.updatePassword(toJS(this.form.value))

    if (result.isSuccess) {
      this.state.success()

      await this.me.reload()

      return
    }

    this.state.failure(result)

    this.form.setStatus(resultToStatus(result))
    if (result.plainErrors.length)
      this.notifications.error(result.plainErrors.join(', '))
  }
}
