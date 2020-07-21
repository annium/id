import { privateClient } from './base'
import { appApi } from './client/appApi'

const privateApi = appApi(privateClient)

export const appService = privateApi
