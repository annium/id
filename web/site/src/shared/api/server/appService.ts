import { appApi } from './client/appApi'
import { privateClient } from './clients'

const privateApi = appApi(privateClient)

export const appService = privateApi
