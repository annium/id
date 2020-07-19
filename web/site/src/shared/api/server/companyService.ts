import { companyApi } from './client/companyApi'
import { privateClient } from './clients'

const privateApi = companyApi(privateClient)

export const appService = {
  getCompanyInfo: privateApi.getCompanyInfo,
}
