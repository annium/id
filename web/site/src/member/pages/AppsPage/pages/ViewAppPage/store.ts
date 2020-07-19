import { AsyncDataState } from '@annium/utils/dist/async'
import { mapResponse } from '@annium/utils/dist/helpers'
import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { App, AppResponseSchema } from 'member/models/App'
import { routes } from 'member/routes'
import { action, observable } from 'mobx'
import { appService } from 'shared/api/server/appService'
import { IMeStore } from 'shared/stores/MeStore'
import { IRouterStore } from 'shared/stores/RouterStore'


export class Store {
  @observable
  public app: AsyncDataState<App> = new AsyncDataState<App>({ id: '', name: '' })

  @lazyInject(services.MeStore)
  public me!: IMeStore

  @lazyInject(services.RouterStore)
  public router!: IRouterStore

  public constructor() {
    delete this.me
    delete this.router
  }

  @action.bound
  public async load() {
    this.app.start()

    this.app.data.id = this.router.parse<{ app: string }>(routes.apps.view).app
    const result = await appService.get(this.app.data.id).then(mapResponse(AppResponseSchema))

    if (result.isSuccess)
      this.app.success(result.data)
    else
      this.app.failure(result)
  }
}
