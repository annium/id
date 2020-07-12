import { createState, State } from '@annium/forms'
import { AsyncDataState, AsyncState } from '@annium/utils/dist/async'
import { mapResponseArray } from '@annium/utils/dist/helpers'
import { INotificationStore } from '@annium/utils/dist/stores'
import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { App, AppResponseSchema } from 'member/models/App'
import { getLog } from 'member/utils/log'
import { action, IObservableValue, observable } from 'mobx'
import { appService } from 'shared/api/server/appService'
// import { accountService } from 'shared/api/server/accountService'
// import { Exchange } from 'shared/api/server/client/shared'
import { IRouterStore } from 'shared/stores/RouterStore'


const log = getLog('AccountsStore')

export class Store {
  @observable
  public apps: AsyncDataState<App[]> = new AsyncDataState<App[]>([])
  @observable
  public form: State<App> = createState<App>({
    id: '',
    name: '',
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
    this.apps.start()
    log('load')

    const result = await appService.list().then(mapResponseArray(AppResponseSchema))

    if (result.isSuccess)
      this.apps.success(result.data)
    else
      this.apps.failure(result)
  }
}
