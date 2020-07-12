import { httpClientFactory, HttpResponse, HttpStatusCode, MiddlewareBase } from '@annium/client-http'
import moment from 'moment'

import { loginService } from './loginService'
import { tokenStorage } from './tokenStorage'

export const appKey = process.env.REACT_APP_APP_ID!
const { protocol, hostname: host, port } = new URL(process.env.REACT_APP_API!)

export const authMiddleware: MiddlewareBase = async next => {
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

  const updateTokenResult = await loginService.updateToken(tokens.refreshToken)

  if (updateTokenResult.isFailure)
    return response

  return next()
}

const getAuthFailureResponse = (error: string) =>
  new HttpResponse(HttpStatusCode.Unauthorized, error, null, [error], {})

export const publicClient = httpClientFactory({
  url: {
    protocol,
    // tslint:disable-next-line: object-literal-sort-keys
    host,
    port: parseInt(port, 10),
  },
})

export const authorization = (headers: {}) => ({
  ...headers,
  get Authorization() {
    const tokens = tokenStorage.get()
    if (!tokens)
      return ''

    return `Bearer ${tokens.accessToken}`
  },
})

export const privateBaseClient = httpClientFactory({
  url: {
    protocol,
    // tslint:disable-next-line: object-literal-sort-keys
    host,
    port: parseInt(port, 10),
  },
  init: {
    headers: authorization({}),
  },
})


export const privateClient = httpClientFactory({
  url: {
    protocol,
    // tslint:disable-next-line: object-literal-sort-keys
    host,
    port: parseInt(port, 10),
  },
  init: {
    headers: authorization({}),
  },
})

privateClient.useMiddleware(authMiddleware)
