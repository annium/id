import { IResultBase } from '@annium/data'
import { AsyncDataState } from '@annium/utils/dist/async'
import { mapResponseArray } from '@annium/utils/dist/helpers'
import { injectable } from 'inversify'
import { Company, CompanyResponseSchema } from 'member/models/Company'
import { action, computed, observable } from 'mobx'
import { companyService } from 'shared/api/server/companyService'
import { getLog } from 'shared/utils/log'


const log = getLog('CompanyStore')

@injectable()
export class CompanyStore implements ICompanyStore {
  @observable
  public items: AsyncDataState<Company[]> = new AsyncDataState<Company[]>([])

  @computed
  public get current(): Company | null {
    return this._current
  }

  @observable
  private _current: Company | null = null

  @action.bound
  public set(company: Company | null): void {
    log('set company to', company)

    this._current = company
  }

  @action.bound
  public async load(query: string): Promise<IResultBase> {
    this.items.start()

    const result = await companyService.findCompanies(query).then(mapResponseArray(CompanyResponseSchema))

    if (result.isSuccess)
      this.items.success(result.data)
    else
      this.items.failure(result)

    return result
  }
}

export interface ICompanyStore {
  items: AsyncDataState<Company[]>
  current: Company | null
  set(company: Company | null): void
  load(query: string): Promise<IResultBase>
}
