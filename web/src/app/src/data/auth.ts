import { combinationFactory, reducerFactory } from '@annium/store'
import { AsyncState, createAsync } from '@annium/utils'
import { pick } from 'lodash'

import { userApi } from '../api/user'
import { context } from '../context'
import { User } from '../models/User'


export type Auth = {
  user: AsyncState<User | undefined>
  access: boolean
}

const user = createAsync<User | undefined>(context, undefined)

const raw = reducerFactory(context, false)
  .action('setAccess', (store, access: boolean) => access)
  .function('load', ({ setAccess }) => async () => {
    user.actions.start({})
    const userResult = await userApi.load()
    if (userResult.isSuccess)
      user.actions.success(userResult)
    else
      user.actions.failure(userResult)
    setAccess(userResult.isSuccess)
  })
  .function('login', ({ setAccess }) => async ({ name, password }: { name: string, password: string }) => {
    user.actions.start({})
    const loginResult = await userApi.login(name, password)
    if (loginResult.isFailure) {
      user.actions.failure(loginResult)
      setAccess(false)

      return
    }

    const userResult = await userApi.load()
    if (userResult.isSuccess)
      user.actions.success(userResult)
    else
      user.actions.failure(userResult)
    setAccess(userResult.isSuccess)
  })
  .function('logout', ({ setAccess }) => async () => {
    user.actions.start({})
    await userApi.logout()
    user.actions.reset({})
    setAccess(false)
  })
  .function('update', ({ setAccess }) => async ({ login, password }: { login: string, password: string }) => {
    user.actions.start({})
    const updateResult = await userApi.update({ login, password, firstName: '', lastName: '', email: '' })
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
  .function('updateToken', ({ setAccess }) => async () => {
    user.actions.start({})
    const updateResult = await userApi.updateToken('') // TODO: real refresh token needed
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
  .build()

export const authActions = pick(raw.actions, ['load', 'login', 'logout', 'update', 'updateToken'])
export const authReducer = combinationFactory().add('user', user.reducer).add('access', raw.reducer).build()
