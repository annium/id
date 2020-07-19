import { injectable } from 'inversify'
import { Company } from 'member/models/Company'
import { action, computed, observable } from 'mobx'
import { getLog } from 'shared/utils/log'


const log = getLog('CompanyStore')

@injectable()
export class CompanyStore implements ICompanyStore {
  @computed
  public get company(): Company | null {
    return this._company
  }

  @observable
  private _company: Company | null = null

  @action.bound
  public set(company: Company): void {
    log('set company to', company)

    this._company = company
  }
}

export interface ICompanyStore {
  company: Company | null
  set(company: Company): void
}
