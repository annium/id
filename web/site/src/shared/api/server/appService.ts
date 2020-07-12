import { HttpResponse } from '@annium/client-http'

import { appApi, AppResponse } from './client/appApi'
import { privateClient } from './clients'

const privateApi = appApi(privateClient)

export const appService = {
  list: (): Promise<HttpResponse<AppResponse[]>> =>
    privateApi.listApps()
}
