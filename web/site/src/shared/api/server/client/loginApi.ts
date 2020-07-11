// imports
import { HttpClient, HttpResponse } from '@annium/client-http'

import {
  TokensResponse,
} from './shared'
// exports
export type LogInRequestBody = {
  login: string
  password: string
}


// api
export const loginApi = (client: HttpClient) => ({
  logIn: (
    appId: string,
    body: LogInRequestBody,
  ): Promise<HttpResponse<TokensResponse>> => client
    .post(`me/${appId}/login`, {}, body),
  updateToken: (
    appId: string,
    refreshToken: string,
  ): Promise<HttpResponse<TokensResponse>> => client
    .put(`me/${appId}/token`, { refreshToken }),
  logOut: (
    appId: string,
  ): Promise<HttpResponse> => client
    .delete(`me/${appId}/logout`, {}),
})
