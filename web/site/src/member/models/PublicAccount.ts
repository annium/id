import { morphism, StrictSchema } from 'morphism'
import { AccountPublicResponse, Exchange } from 'shared/api/server/client/shared'

import { User, UserResponseSchema } from './User'

export type PublicAccount = {
  id: string
  user: User
  name: string
  exchange: Exchange
  isTest: boolean
}

export const AccountPublicResponseSchema: StrictSchema<PublicAccount, AccountPublicResponse> = {
  id: 'id',
  name: 'name',
  user: x => morphism(UserResponseSchema, x.user),
  exchange: 'exchange',
  isTest: 'isTest',
}
