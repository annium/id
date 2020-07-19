import { HttpResponse } from '@annium/client-http'

import { meApi, RegisterMeRequest } from './client/meApi'
import { TokensResponse } from './client/shared'
import { appKey, privateClient, publicClient } from './clients'
import { tokenStorage } from './tokenStorage'

const publicApi = meApi(publicClient)
const privateApi = meApi(privateClient)

export const meService = {
  registerMe: (email: string, login: string, referralId: string | null): Promise<HttpResponse> => {
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
  getMe: privateApi.getMe,
  updateMyProfile:  privateApi.updateMyProfile,
  updateMyPassword: privateApi.updateMyPassword,
}
