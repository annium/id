import { userApi } from './client/userApi'
import { privateClient } from './clients'

const privateApi = userApi(privateClient)

export const userService = {
  getUser: privateApi.getUser,
}
