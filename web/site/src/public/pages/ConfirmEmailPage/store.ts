import { IResultBase, Result } from '@annium/data'
import { AsyncDataState } from '@annium/utils/dist/async'
import { action, observable } from 'mobx'
import { lazyInject } from 'public/config/di/container'
import { services } from 'public/config/di/services'
import { meService } from 'shared/api/server/meService'
import { IMeStore } from 'shared/stores/MeStore'
import { IRouterStore } from 'shared/stores/RouterStore'


export enum ConfirmationStatus {
  None,
  InvalidLink,
  Failure,
  Success,
}

export class Store {
  @observable
  public state: AsyncDataState<ConfirmationStatus> = new AsyncDataState<ConfirmationStatus>(ConfirmationStatus.None)
  @lazyInject(services.MeStore)
  public me!: IMeStore
  @lazyInject(services.RouterStore)
  public router!: IRouterStore

  public constructor() {
    delete this.me
    delete this.router
  }

  @action.bound
  public async init(): Promise<void> {
    const params = this.router.parse<{ id: string }>('/confirm-email')

    // immediate failure if no id
    if (!params.id) {
      this.state.failure(Result.create(), ConfirmationStatus.InvalidLink)

      return
    }

    this.state.start()

    const registrationResult = await this.confirmAndRegister(params.id)
    if (registrationResult.hasErrors) {
      this.state.failure(registrationResult, ConfirmationStatus.Failure)

      return
    }

    this.state.success(ConfirmationStatus.Success)
    // TODO: use Routes
    setTimeout(() => {
      this.router.go('/member/profile')
    }, 5000)
  }

  private async confirmAndRegister(id: string): Promise<IResultBase> {
    const confirmationResult = await meService.confirmEmail(id)
    if (confirmationResult.isFailure)
      return confirmationResult

    const userResult = await meService.load()
    if (userResult.isFailure)
      return userResult

    console.log('load me')
    await this.me.load()

    return this.me.state
  }
}
