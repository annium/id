import { injectable } from 'inversify'
import { App } from 'member/models/App'
import { action, computed, observable } from 'mobx'
import { getLog } from 'shared/utils/log'


const log = getLog('AppStore')

@injectable()
export class AppStore implements IAppStore {
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
}

export interface IAppStore {
  app: App | null
  set(app: App): void
}
