import { HttpResponse } from '@annium/client-http'

import { app, privateBaseClient, privateClient, publicClient } from './base'
import { loginApi } from './client/loginApi'
import { TokensResponse } from './client/shared'
import { tokenStorage } from './tokenStorage'

const publicApi = loginApi(publicClient)
const privateApi = loginApi(privateClient)
const privateBaseApi = loginApi(privateBaseClient)

export const loginService = {
  login: async (login: string, password: string): Promise<HttpResponse<TokensResponse>> => {
    const response = await publicApi.logIn(app.key, { login, password })

    if (response.isSuccess)
      tokenStorage.set(response.data)
    else
      tokenStorage.clear()

    return response
  },
  logout: async (): Promise<HttpResponse> => {
    const responsePromise = privateApi.logOut(app.key)

    // this one is needed to prevent access to tokens immediately
    tokenStorage.clear()

    return responsePromise
  },
  updateToken: async (refreshToken: string): Promise<HttpResponse<TokensResponse>> => {
    const response = await privateBaseApi.updateToken(app.key, refreshToken)

    if (response.isSuccess)
      tokenStorage.set(response.data)
    else
      tokenStorage.clear()

    return response
  },
}
