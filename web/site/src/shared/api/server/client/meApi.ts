// imports
import { HttpClient, HttpResponse } from '@annium/client-http'

import {
  TokensResponse,
} from './shared'
// exports
export type ConfirmMyEmailRequestBody = {
  id: string
}

export type MeResponse = {
  id: string
  login: string
  email: string
  referralId: string | null
}

export type RegisterMeRequest = {
  server: string
  email: string
  login: string
  referralId: string | null
}

export type RestoreMyAccessRequestBody = {
  server: string
  email: string
}

export type UpdateMyPasswordRequest = {
  password: string
}

export type UpdateMyProfileRequest = {
  login: string
  email: string
}


// api
export const meApi = (client: HttpClient) => ({
  registerMe: (
    body: RegisterMeRequest,
  ): Promise<HttpResponse> => client
    .post(`me`, {}, body),
  confirmMyEmail: (
    appId: string,
    body: ConfirmMyEmailRequestBody,
  ): Promise<HttpResponse<TokensResponse>> => client
    .post(`me/${appId}/confirm-email`, {}, body),
  restoreMyAccess: (
    appId: string,
    body: RestoreMyAccessRequestBody,
  ): Promise<HttpResponse> => client
    .post(`me/${appId}/restore-access`, {}, body),
  getMe: (
  ): Promise<HttpResponse<MeResponse>> => client
    .get(`me`, {}),
  updateMyProfile: (
    body: UpdateMyProfileRequest,
  ): Promise<HttpResponse> => client
    .put(`me/profile`, {}, body),
  updateMyPassword: (
    body: UpdateMyPasswordRequest,
  ): Promise<HttpResponse> => client
    .put(`me/password`, {}, body),
  unregisterMe: (
  ): Promise<HttpResponse> => client
    .delete(`me`, {}),
})
