import { Response } from '@annium/server-http'

import { User } from '../models/User'
import { UserToken } from '../models/UserToken'

import { privateApi, publicApi } from './api'
import { UserPayload } from './payloads/UserPayload'


export const userApi = {
  register: (user: UserPayload): Promise<Response<User>> =>
    publicApi.put('me', undefined, user),
  load: (): Promise<Response<User>> =>
    privateApi.get<User>('me'),
  login: (login: string, password: string): Promise<Response<UserToken>> =>
    publicApi.post('me/login', undefined, { login, password }),
  logout: (): Promise<Response> =>
    privateApi.post('me/logout'),
  updateToken: (refreshToken: string): Promise<Response<UserToken>> =>
    publicApi.post('me/token', undefined, { refreshToken }),
  update: (user: UserPayload): Promise<Response<User>> =>
    privateApi.post('me', undefined, user),
  unregister: (): Promise<Response> =>
    privateApi.delete('me'),
}
