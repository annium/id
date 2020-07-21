import { IResultBase } from '@annium/data/dist'
import { AsyncDataState } from '@annium/utils/dist/async'
import { mapResponseArray } from '@annium/utils/dist/helpers'
import { injectable } from 'inversify'
import { App, AppResponseSchema } from 'member/models/App'
import { action, computed, observable } from 'mobx'
import { appService } from 'shared/api/server/appService'
import { getLog } from 'shared/utils/log'


const log = getLog('AppStore')

@injectable()
export class AppStore implements IAppStore {
  @observable
  public items: AsyncDataState<App[]> = new AsyncDataState<App[]>([])

  @computed
  public get current(): App | null {
    return this._current
  }

  @observable
  private _current: App | null = null

  @action.bound
  public set(app: App | null): void {
    log('set app to', app)

    this._current = app
  }

  @action.bound
  public async load(query: string): Promise<IResultBase> {
    this.items.start()

    const result = await appService.findApps(query).then(mapResponseArray(AppResponseSchema))

    if (result.isSuccess)
      this.items.success(result.data)
    else
      this.items.failure(result)

    return result
  }
}

export interface IAppStore {
  items: AsyncDataState<App[]>
  current: App | null
  set(app: App | null): void
  load(query: string): Promise<IResultBase>
}
