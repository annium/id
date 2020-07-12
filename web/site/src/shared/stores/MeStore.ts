import { Result } from '@annium/data'
import { AsyncState } from '@annium/utils/dist/async'
import { injectable } from 'inversify'
import { action, computed, observable } from 'mobx'
import { loginService } from 'shared/api/server/loginService'
import { meService } from 'shared/api/server/meService'
import { tokenStorage } from 'shared/api/server/tokenStorage'
import { User } from 'shared/models/User'
import { getLog } from 'shared/utils/log'


const log = getLog('MeStore')
@injectable()
export class MeStore implements IMeStore {
  @computed
  public get hasLoadedOnce(): boolean { return this._hasLoadedOnce }
  @computed
  public get hasAccess(): boolean { return this.state.isSuccess }
  @observable
  public user: User | null = null
  @observable
  public state: AsyncState = new AsyncState()
  @observable
  private _hasLoadedOnce: boolean = false

  @action.bound
  public load() {
    log('load')

    return this._load(false)
  }

  @action.bound
  public reload() {
    log('reload')

    return this._load(true)
  }

  @action.bound
  public async logout() {
    log('logout', 'start')
    this.state.start()

    log('logout', 'api start')
    await loginService.logout()
    log('logout', 'api end')

    this.state.reset()
    log('logout', 'reset')
  }

  private async _load(force: boolean): Promise<void> {
    // fail immediately if no tokens
    if (!tokenStorage.get()) {
      log('_load', 'no tokens')
      const failure = Result.create().error('Tokens missing')
      this.state.failure(failure)
      this._hasLoadedOnce = true

      return
    }

    // cache call, if allowed
    if (!force && this.hasAccess) {
      log('_load', 'not force, had access - cached success')
      this.state.success()
      this._hasLoadedOnce = true

      return
    }

    log('_load', 'start')
    this.state.start()

    log('_load', 'api start')
    const result = await meService.getMe()
    log('_load', 'api end')

    if (result.isSuccess) {
      this.user = result.data
      this.state.success()
      log('_load', 'success')
    } else {
      log('_load', 'logout api start')
      await loginService.logout()
      log('_load', 'logout api end')
      this.state.failure(result)
      log('_load', 'failure')
    }
    this._hasLoadedOnce = true
    log('_load', 'end')
  }
}

export interface IMeStore {
  hasLoadedOnce: boolean
  hasAccess: boolean
  user: User | null
  state: AsyncState
  load(): Promise<void>
  reload(): Promise<void>
  logout(): Promise<void>
}
