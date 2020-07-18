import { IRouterStore as IRouterStoreBase, RouterStore as RouterStoreBase } from '@annium/utils/dist/stores'
import { injectable } from 'inversify'
import { action, computed } from 'mobx'
import { Routes as PublicRoutes } from 'public/routes'


@injectable()
export class RouterStore extends RouterStoreBase implements IRouterStore {
  @computed
  public get isCurrentLocationPublic(): boolean {
    return this.isLocationPublic(this.location.pathname)
  }

  public isLocationPublic(path: string): boolean {
    return Object.values(PublicRoutes).map(String).includes(path)
  }

  @action
  public goToHomeOrStartup(): void {
    // if startup is public - go home, else - go to startup
    if (this.isLocationPublic(this.startup.pathname))
      // TODO: add permissions data from server and make this redirect smarter
      this.go('/member')
    else
      this.go(this.startup)
  }

  @action
  public goToLogin(): void {
    this.go('/login')
  }
}

export interface IRouterStore extends IRouterStoreBase {
  isCurrentLocationPublic: boolean
  isLocationPublic(path: string): boolean
  goToHomeOrStartup(): void
  goToLogin(): void
}
