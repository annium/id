import { combinationFactory, reducerFactory } from '@annium/store'
import { AsyncState, createAsync, storage } from '@annium/utils'
import { pick } from 'lodash'
import moment from 'moment'

import { UserPayload } from '../api/payloads/UserPayload'
import { userApi } from '../api/user'
import { context } from '../context'
import { User } from '../models/User'
import { UserToken } from '../models/UserToken'


export type Auth = {
  hasAccess: boolean
  user: AsyncState<User | null>
  token: AsyncState<UserToken | null>
}

const storageKeys = {
  login: 'login',
}
const user = createAsync<User | null>(context, null)
const token = createAsync<UserToken | null>(context, storage.get<UserToken>(storageKeys.login))

const raw = reducerFactory(context, false)
  .action('setAccess', (_, hasAccess: boolean) => hasAccess)
  .function('register', ({ setAccess }) => async (payload: UserPayload) => {
    user.actions.start({})

    const registerResult = await userApi.register(payload)
    if (registerResult.isSuccess)
      user.actions.success(registerResult)
    else {
      user.actions.failure(registerResult)
      token.actions.reset({})
      setAccess(false)

      return
    }

    await raw.actions.login({ login: payload.login, password: payload.password })
  })
  .function('load', ({ setAccess }) => async () => {
    user.actions.start({})
    const userResult = await userApi.load()
    setAccess(userResult.isSuccess)
    if (userResult.isSuccess)
      user.actions.success(userResult)
    else
      user.actions.failure(userResult)
  })
  .function('login', ({ setAccess }) => async ({ login, password }: { login: string, password: string }) => {
    token.actions.start({})

    // try login
    const loginResult = await userApi.login(login, password)
    if (loginResult.isSuccess) {
      token.actions.success(loginResult)
      storage.set(storageKeys.login, loginResult.data)
    } else {
      token.actions.failure(loginResult)
      user.actions.reset({})
      setAccess(false)
      storage.remove(storageKeys.login)

      return
    }

    await raw.actions.load({})
  })
  .function('logout', ({ setAccess }) => async () => {
    user.actions.start({})
    await userApi.logout()
    user.actions.reset({})
    token.actions.reset({})
    setAccess(false)
    storage.remove(storageKeys.login)
  })
  .function('update', ({ setAccess }) => async (payload: UserPayload) => {
    user.actions.start({})
    const updateResult = await userApi.update(payload)
    if (updateResult.isFailure) {
      user.actions.failure(updateResult)

      return
    }

    const userResult = await userApi.load()
    if (userResult.isSuccess)
      user.actions.success(userResult)
    else
      user.actions.failure(userResult)
    setAccess(userResult.isSuccess)
  })
  .function('updateToken', (_, getState) => async () => {
    const tokens = getState().auth.token.data
    if (!tokens)
      throw new Error('No user token available to perform token update')

    if (moment(tokens.refreshTokenExpires).isBefore(moment()))
      throw new Error('Refresh token is expired. Need to login')

    token.actions.start({})
    const updateResult = await userApi.updateToken(tokens.refreshToken)
    if (updateResult.isSuccess)
      token.actions.success(updateResult)
    else
      token.actions.failure(updateResult)
  })
  .function('unregister', ({ setAccess }, getState) => async () => {
    user.actions.start({})

    const unregisterResult = await userApi.unregister()
    if (unregisterResult.isSuccess) {
      user.actions.reset({})
      token.actions.reset({})
      setAccess(false)
      storage.remove(storageKeys.login)
    }
    else
      user.actions.success({ data: getState().auth.user.data })
  })
  .build()

export const authActions = pick(raw.actions, ['load', 'login', 'logout', 'update', 'updateToken'])
export const authReducer = combinationFactory()
  .add('hasAccess', raw.reducer)
  .add('user', user.reducer)
  .add('token', token.reducer)
  .build()
