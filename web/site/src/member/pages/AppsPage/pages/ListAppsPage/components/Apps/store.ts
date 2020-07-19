import { AsyncDataState } from '@annium/utils/dist/async'
import { mapResponseArray } from '@annium/utils/dist/helpers'
import { INotificationStore } from '@annium/utils/dist/stores'
import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { App, AppResponseSchema } from 'member/models/App'
import { action, observable } from 'mobx'
import { appService } from 'shared/api/server/appService'


export class Store {
  @observable
  public apps: AsyncDataState<App[]> = new AsyncDataState<App[]>([])
  @lazyInject(services.NotificationStore)
  public notifications!: INotificationStore

  public constructor() {
    delete this.notifications
  }

  @action.bound
  public async load() {
    this.apps.start()

    const result = await appService.listApps().then(mapResponseArray(AppResponseSchema))

    if (result.isSuccess)
      this.apps.success(result.data)
    else {
      this.apps.failure(result)
      this.notifications.error(result.plainErrors.join(', '))
    }
  }
}
