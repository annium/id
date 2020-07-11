import { AsyncDataState } from '@annium/utils/dist/async'
import { mapResponseArray } from '@annium/utils/dist/helpers'
import { AccountPublicResponseSchema, PublicAccount } from 'member/models/PublicAccount'
import { action, observable } from 'mobx'
import { accountService } from 'shared/api/server/accountService'


export class Store {
  @observable
  public accounts: AsyncDataState<PublicAccount[]> = new AsyncDataState<PublicAccount[]>([])

  @action.bound
  public async load(userId: string) {
    if (!userId)
      return

    this.accounts.start()

    const result = await accountService.listAccounts(userId)
      .then(mapResponseArray(AccountPublicResponseSchema))

    if (result.isSuccess)
      this.accounts.success(result.data)
    else
      this.accounts.failure(result)
  }
}
