import { privateClient } from './base'
import { userApi } from './client/userApi'

const privateApi = userApi(privateClient)

export const userService = privateApi
