import { AsyncDataState } from '@annium/utils/dist/async'
import { mapResponse } from '@annium/utils/dist/helpers'
import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { User, UserResponseSchema } from 'member/models/User'
import { routes } from 'member/routes'
import { action, observable } from 'mobx'
import { userService } from 'shared/api/server/userService'
import { IRouterStore } from 'shared/stores/RouterStore'


export class Store {
  @observable
  public user: AsyncDataState<User> = new AsyncDataState<User>({ id: '', login: '' })

  @lazyInject(services.RouterStore)
  public router!: IRouterStore

  public readonly appId: string

  public constructor() {
    delete this.router
    this.appId = this.router.parse<{ app: string }>(routes.apps.users.user.view).app
  }

  @action.bound
  public async load() {
    this.user.start()

    this.user.data.id = this.router.parse<{ user: string }>(routes.apps.users.user.view).user
    const result = await userService.get(this.user.data.id).then(mapResponse(UserResponseSchema))

    if (result.isSuccess)
      this.user.success(result.data)
    else
      this.user.failure(result)
  }
}
