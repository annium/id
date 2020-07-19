import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { action, observable } from 'mobx'
import { IMeStore } from 'shared/stores/MeStore'
import { IRouterStore } from 'shared/stores/RouterStore'


export class Store {
  @lazyInject(services.MeStore)
  public me!: IMeStore
  @lazyInject(services.RouterStore)
  public router!: IRouterStore

  @observable
  // @ts-ignore
  private readonly _fake: boolean = false

  public constructor() {
    delete this.me
    delete this.router
  }

  @action.bound
  public async goHome() {
    this.router.go('/member')
  }

  @action.bound
  public async logout() {
    await this.me.logout()

    this.router.goToLogin()
  }
}
