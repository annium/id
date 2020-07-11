import { accountApi } from './client/accountApi'
import { privateClient } from './clients'

export const accountService = accountApi(privateClient)
