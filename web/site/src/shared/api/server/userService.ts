import { userApi } from './client/userApi'
import { privateClient } from './clients'

export const userService = userApi(privateClient)
