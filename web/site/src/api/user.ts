import { HttpResponse } from '@annium/client-http'

import { User } from '../models/User'
import { UserToken } from '../models/UserToken'

import { privateApi, publicApi } from './api'
import { UserPayload } from './payloads/UserPayload'


export const userApi = {
  register: (user: UserPayload): Promise<HttpResponse<User>> =>
    publicApi.put('me', undefined, user),
  load: (): Promise<HttpResponse<User>> =>
    privateApi.get<User>('me'),
  login: (login: string, password: string): Promise<HttpResponse<UserToken>> =>
    publicApi.post('me/id/login', undefined, { login, password }),
  logout: (): Promise<HttpResponse> =>
    privateApi.post('me/id/logout'),
  updateToken: (refreshToken: string): Promise<HttpResponse<UserToken>> =>
    publicApi.post('me/id/token', { refreshToken }),
  update: (user: UserPayload): Promise<HttpResponse<User>> =>
    privateApi.post('me', undefined, user),
  unregister: (): Promise<HttpResponse> =>
    privateApi.delete('me'),
}
