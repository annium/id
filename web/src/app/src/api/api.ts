import { httpClientFactory, HttpStatusCode } from '@annium/client-http'
import { storage } from '@annium/utils'
import moment from 'moment'

import { storageKeys } from '../data/auth'
import { UserToken } from '../models/UserToken'

import { userApi } from './user'

const { protocol, hostname: host, port } = new URL(process.env.REACT_APP_API_URL || window.location.toString())

export const publicApi = httpClientFactory({
  url: {
    protocol,
    // tslint:disable-next-line: object-literal-sort-keys
    host,
    port: parseInt(port, 10),
  },
})

export const privateApi = httpClientFactory({
  url: {
    protocol,
    // tslint:disable-next-line: object-literal-sort-keys
    host,
    port: parseInt(port, 10),
  },
  init: {
    headers: {
      get Authorization() {
        return `Bearer ${storage.get<UserToken>(storageKeys.token)!.accessToken}`
      },
    },
  },
})

privateApi.useMiddleware(async next => {
  const response = await next()

  // if response succeed or any failure except Unauthorized - return response as is
  if (response.isSuccess || response.status !== HttpStatusCode.Unauthorized)
    return response

  // if unauthorized - try refresh token and retry
  const tokens = storage.get<UserToken>(storageKeys.token)
  if (!tokens)
    throw new Error('No user token available to perform token update')

  if (moment(tokens.refreshTokenExpires).isBefore(moment()))
    throw new Error('Refresh token is expired. Need to login')

  const updateTokenResult = await userApi.updateToken(tokens.refreshToken)

  // if update failed - return initial result
  if (updateTokenResult.isFailure) {
    storage.remove(storageKeys.token)

    return response
  }

  // if update succeed - retry
  storage.set(storageKeys.token, updateTokenResult.data)

  return next()
})
