import { AsyncDataState } from '@annium/utils/dist/async'
import { mapResponseArray } from '@annium/utils/dist/helpers'
import { User, UserResponseSchema } from 'member/models/User'
import { action, observable } from 'mobx'
import { userService } from 'shared/api/server/userService'


export class Store {
  @observable
  public users: AsyncDataState<User[]> = new AsyncDataState<User[]>([])

  @action.bound
  public async load(query: string) {
    if (query.length < 3) {
      this.users.success([])

      return
    }

    this.users.start()

    const result = await userService.findUsers(query)
      .then(mapResponseArray(UserResponseSchema))

    if (result.isSuccess)
      this.users.success(result.data)
    else
      this.users.failure(result)
  }
}
