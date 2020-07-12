import { HttpResponse } from '@annium/client-http'

import { loginApi } from './client/loginApi'
import { TokensResponse } from './client/shared'
import { appKey, privateBaseClient, privateClient, publicClient } from './clients'
import { tokenStorage } from './tokenStorage'

const publicApi = loginApi(publicClient)
const privateApi = loginApi(privateClient)
const privateBaseApi = loginApi(privateBaseClient)

export const loginService = {
  login: async (login: string, password: string): Promise<HttpResponse<TokensResponse>> => {
    const response = await publicApi.logIn(appKey, { login, password })

    if (response.isSuccess)
      tokenStorage.set(response.data)
    else
      tokenStorage.clear()

    return response
  },
  logout: async (): Promise<HttpResponse> => {
    const responsePromise = privateApi.logOut(appKey)

    // this one is needed to prevent access to tokens immediately
    tokenStorage.clear()

    return responsePromise
  },
  updateToken: async (refreshToken: string): Promise<HttpResponse<TokensResponse>> => {
    const response = await privateBaseApi.updateToken(appKey, refreshToken)

    if (response.isSuccess)
      tokenStorage.set(response.data)
    else
      tokenStorage.clear()

    return response
  },
}
