import { HttpResponse } from '@annium/client-http'

import { meApi, MeResponse, RegisterMeRequest } from './client/meApi'
import { TokensResponse } from './client/shared'
import { appKey, privateClient, publicClient } from './clients'
import { tokenStorage } from './tokenStorage'

const publicApi = meApi(publicClient)
const privateApi = meApi(privateClient)

export const meService = {
  register: (email: string, login: string, referralId: string | null): Promise<HttpResponse> => {
    const payload: RegisterMeRequest = { server: window.location.origin, email, login, referralId }

    return publicApi.registerMe(payload)
  },
  confirmEmail: async (id: string): Promise<HttpResponse<TokensResponse>> => {
    const response = await publicApi.confirmMyEmail(appKey, { id })

    if (response.isSuccess)
      tokenStorage.set(response.data)
    else
      tokenStorage.clear()

    return response
  },
  restoreAccess: (email: string): Promise<HttpResponse> =>
    publicApi.restoreMyAccess(appKey, { server: window.location.origin, email }),
  getMe: (): Promise<HttpResponse<MeResponse>> =>
    privateApi.getMe(),
  updateProfile: async (request: { login: string, email: string }): Promise<HttpResponse> =>
    privateApi.updateMyProfile(request),
  updatePassword: async (request: { password: string }): Promise<HttpResponse> =>
    privateApi.updateMyPassword(request),
  load: (): Promise<HttpResponse<MeResponse>> =>
    privateApi.getMe(),
}
