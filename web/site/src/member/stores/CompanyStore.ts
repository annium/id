import { AsyncDataState } from '@annium/utils/dist/async'
import { mapResponseArray } from '@annium/utils/dist/helpers'
import { INotificationStore } from '@annium/utils/dist/stores'
import { injectable } from 'inversify'
import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { Company, CompanyResponseSchema } from 'member/models/Company'
import { action, computed, observable } from 'mobx'
import { companyService } from 'shared/api/server/companyService'
import { getLog } from 'shared/utils/log'


const log = getLog('CompanyStore')

@injectable()
export class CompanyStore implements ICompanyStore {
  @observable
  public companies: AsyncDataState<Company[]> = new AsyncDataState<Company[]>([])

  @lazyInject(services.NotificationStore)
  public notifications!: INotificationStore

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

  @action.bound
  public async load(query: string): Promise<void> {
    this.companies.start()

    const result = await companyService.findCompanies(query).then(mapResponseArray(CompanyResponseSchema))

    if (result.isSuccess)
      this.companies.success(result.data)
    else {
      this.companies.failure(result)
      this.notifications.error(result.plainErrors.join(', '))
    }
  }
}

export interface ICompanyStore {
  company: Company | null
  set(company: Company): void
  load(query: string): Promise<void>
}
