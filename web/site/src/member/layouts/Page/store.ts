import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { action, computed, observable } from 'mobx'
import { IMeStore } from 'shared/stores/MeStore'
import { IRouterStore } from 'shared/stores/RouterStore'


export class Store {
  @lazyInject(services.MeStore)
  public me!: IMeStore
  @lazyInject(services.RouterStore)
  public router!: IRouterStore

  @computed
  public get isSidebarOpen(): boolean {
    return this._isSidebarOpen
  }

  @observable
  private _isSidebarOpen: boolean = false

  public constructor() {
    delete this.me
    delete this.router
  }

  @action.bound
  public toggleSidebar() {
    this._isSidebarOpen = !this._isSidebarOpen
  }

  @action.bound
  public async logout() {
    await this.me.logout()

    this.router.goToLogin()
  }
}
