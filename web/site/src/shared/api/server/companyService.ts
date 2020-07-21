import { privateClient } from './base'
import { companyApi } from './client/companyApi'

const privateApi = companyApi(privateClient)

export const companyService = privateApi
