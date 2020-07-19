import { HttpResponse } from '@annium/client-http'

import { UserResponse } from './client/shared'
import { userApi } from './client/userApi'
import { privateClient } from './clients'

const privateApi = userApi(privateClient)

export const userService = {
  get: (id: string): Promise<HttpResponse<UserResponse>> =>
    privateApi.getUser(id),
}
