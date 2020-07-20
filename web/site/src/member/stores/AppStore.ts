import { AsyncDataState } from '@annium/utils/dist/async'
import { mapResponseArray } from '@annium/utils/dist/helpers'
import { INotificationStore } from '@annium/utils/dist/stores'
import { injectable } from 'inversify'
import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { App, AppResponseSchema } from 'member/models/App'
import { action, computed, observable } from 'mobx'
import { appService } from 'shared/api/server/appService'
import { getLog } from 'shared/utils/log'


const log = getLog('AppStore')

@injectable()
export class AppStore implements IAppStore {
  @observable
  public apps: AsyncDataState<App[]> = new AsyncDataState<App[]>([])

  @lazyInject(services.NotificationStore)
  public notifications!: INotificationStore

  @computed
  public get app(): App | null {
    return this._app
  }

  @observable
  private _app: App | null = null

  @action.bound
  public set(app: App): void {
    log('set app to', app)

    this._app = app
  }

  @action.bound
  public async load(query: string): Promise<void> {
    this.apps.start()

    const result = await appService.findApps(query).then(mapResponseArray(AppResponseSchema))

    if (result.isSuccess)
      this.apps.success(result.data)
    else {
      this.apps.failure(result)
      this.notifications.error(result.plainErrors.join(', '))
    }
  }
}

export interface IAppStore {
  app: App | null
  set(app: App): void
  load(query: string): Promise<void>
}
