import { HttpClient, HttpResponse, HttpStatusCode, MiddlewareBase } from '@annium/client-http'
import moment from 'moment'

import { loginApi } from '../client/loginApi'
import { tokenStorage } from '../tokenStorage'

import { app } from './settings'

export const authorization = (headers: {}) => ({
  ...headers,
  get Authorization() {
    const tokens = tokenStorage.get()
    if (!tokens)
      return ''

    return `Bearer ${tokens.accessToken}`
  },
})

export const getAuthMiddleware = (baseClient: HttpClient): MiddlewareBase => async next => {
  const response = await next()

  // if response succeed or any failure except Unauthorized - return response as is
  if (response.isSuccess || response.status !== HttpStatusCode.Unauthorized)
    return response

  const tokens = tokenStorage.get()
  if (!tokens)
    return getAuthFailureResponse('No user token available to perform token update')

  // if unauthorized - try refresh token and retry
  if (moment(tokens.refreshTokenExpires).isBefore(moment()))
    return getAuthFailureResponse('Refresh token is expired. Need to login')

  const updateTokenResult = await loginApi(baseClient).updateToken(app.key, tokens.refreshToken)

  if (updateTokenResult.isFailure)
    return response

  return next()
}

const getAuthFailureResponse = (error: string) =>
  new HttpResponse(HttpStatusCode.Unauthorized, error, null, [error], {})

