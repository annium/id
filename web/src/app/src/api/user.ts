import { Response } from '@annium/server-http'

import { User } from '../models/User'

import { api } from './api'
import { UserPayload } from './payloads/UserPayload'


export const userApi = {
  register: (user: UserPayload): Promise<Response> =>
    api.put('me', undefined, user),

  login: (name: string, password: string): Promise<Response> =>
    api.post('me/login', undefined, { name, password }),

  load: (): Promise<Response<User>> =>
    api.get<User>('me'),

  logout: (): Promise<Response> =>
    api.post('me/logout'),

  updateToken: (refreshToken: string): Promise<Response> =>
    api.post('me/token', undefined, { refreshToken }),

  update: (user: UserPayload): Promise<Response> =>
    api.post('me', undefined, user),

  unregister: (): Promise<Response> =>
    api.delete('me'),
}
