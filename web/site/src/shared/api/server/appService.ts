import { HttpResponse } from '@annium/client-http'

import { appApi, AppResponse, CreateAppRequest, UpdateAppRequestBody } from './client/appApi'
import { privateClient } from './clients'

const privateApi = appApi(privateClient)

export const appService = {
  list: (): Promise<HttpResponse<AppResponse[]>> =>
    privateApi.listApps(),
  listMy: (): Promise<HttpResponse<AppResponse[]>> =>
    privateApi.listMyApps(),
  create: (body: CreateAppRequest): Promise<HttpResponse<string>> =>
    privateApi.createApp(body),
  update: (id: string, body: UpdateAppRequestBody): Promise<HttpResponse> =>
    privateApi.updateApp(id, body),
  delete: (id: string): Promise<HttpResponse> =>
    privateApi.deleteApp(id),
}
