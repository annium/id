// imports
import { HttpClient, HttpResponse } from '@annium/client-http'



// exports

export type AppResponse = {
  id: string
  name: string
}

export type CreateAppRequest = {
  name: string
}

export type UpdateAppRequestBody = {
  name: string
}


// api
export const appApi = (client: HttpClient) => ({
  createApp: (
    body: CreateAppRequest,
  ): Promise<HttpResponse<string>> => client
    .post(`apps`, {}, body),
  listApps: (
  ): Promise<HttpResponse<AppResponse[]>> => client
    .get(`apps`, {}),
  listMyApps: (
  ): Promise<HttpResponse<AppResponse[]>> => client
    .get(`apps/my`, {}),
  getApp: (
    appId: string,
  ): Promise<HttpResponse<AppResponse>> => client
    .get(`apps/${appId}`, {}),
  getAppApiToken: (
    appId: string,
  ): Promise<HttpResponse<string>> => client
    .get(`apps/${appId}/token`, {}),
  updateApp: (
    appId: string,
    body: UpdateAppRequestBody,
  ): Promise<HttpResponse> => client
    .put(`apps/${appId}`, {}, body),
  setAppOwner: (
    appId: string,
    newOwnerId: string,
  ): Promise<HttpResponse> => client
    .put(`apps/${appId}/owner/${newOwnerId}`, {}),
  updateAppApiToken: (
    appId: string,
  ): Promise<HttpResponse<string>> => client
    .put(`apps/${appId}/token`, {}),
  deleteApp: (
    appId: string,
  ): Promise<HttpResponse> => client
    .delete(`apps/${appId}`, {}),
})
