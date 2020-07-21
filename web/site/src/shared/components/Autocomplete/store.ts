import { IResultBase } from '@annium/data/dist'
import { AsyncState } from '@annium/utils/dist/async'
import { action, computed, observable } from 'mobx'
import { getLog } from 'shared/utils/log'

const log = getLog('Autocomplete.Store')

export class Store<T> {
  @observable
  public readonly state: AsyncState = new AsyncState()

  @computed
  public get query(): string {
    return this._query
  }

  @computed
  public get value(): T | null {
    return this._value
  }

  private readonly _find: (query: string) => Promise<IResultBase>
  @observable
  private _query: string = ''
  private _value: T | null = null

  public constructor(
    value: T | null,
    find: (query: string) => Promise<IResultBase>,
  ) {
    this._find = find
  }

  @action.bound
  public async load() {
    this.state.start()

    log('load with query:', this.query)

    const result = await this._find(this.query)

    if (result.hasErrors)
      this.state.failure(result)
    else
      this.state.success()
  }

  @action.bound
  public async setQuery(query: string) {
    log('set query:', query)
    this._query = query
  }

  @action.bound
  public async setValue(value: T | null) {
    log('set value:', value)
    this._value = value
  }
}
