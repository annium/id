import { AsyncDataState } from '@annium/utils/dist/async'
import { mapResponseArray } from '@annium/utils/dist/helpers'
import { INotificationStore } from '@annium/utils/dist/stores'
import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { App, AppResponseSchema } from 'member/models/App'
import { action, IObservableValue, observable, runInAction } from 'mobx'
import { appService } from 'shared/api/server/appService'


export class Store {
  @observable
  public apps: AsyncDataState<App[]> = new AsyncDataState<App[]>([])
  @observable
  public deleteCandidate: IObservableValue<string | null> = observable.box(null)
  @lazyInject(services.NotificationStore)
  public notifications!: INotificationStore

  public constructor() {
    delete this.notifications
  }

  @action.bound
  public async load() {
    this.apps.start()

    const result = await appService.listMyApps().then(mapResponseArray(AppResponseSchema))

    if (result.isSuccess)
      this.apps.success(result.data)
    else {
      this.apps.failure(result)
      this.notifications.error(result.plainErrors.join(', '))
    }
  }

  @action.bound
  public setDeleteCandidate(candidate: string | null) {
    return () => runInAction(() => {
      this.deleteCandidate.set(candidate)
    })
  }

  @action.bound
  public async delete() {
    if (!this.deleteCandidate)
      return

    this.apps.start()

    const id = this.deleteCandidate.get()!
    this.deleteCandidate.set(null)

    const result = await appService.deleteApp(id)

    if (result.isFailure) {
      this.apps.success(this.apps.data)

      if (result.plainErrors.length)
        this.notifications.error(result.plainErrors.join(', '))

      return
    }

    await this.load()
  }
}
