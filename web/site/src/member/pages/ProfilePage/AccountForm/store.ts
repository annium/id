import { createState, resultToStatus, State } from '@annium/forms'
import { AsyncState } from '@annium/utils/dist/async'
import { INotificationStore } from '@annium/utils/dist/stores'
import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { action, observable, toJS } from 'mobx'
import { meService } from 'shared/api/server/meService'
import { IMeStore } from 'shared/stores/MeStore'


export type Data = { login: string; email: string }

export class Store {
  @observable
  public form: State<Data> = createState<Data>({
    login: '',
    email: '',
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
  public async load() {
    this.form.setValue(this.me.user!)
  }

  @action.bound
  public async save() {
    this.state.start()

    const payload = toJS(this.form.value)
    const result = await meService.updateProfile(payload)

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
