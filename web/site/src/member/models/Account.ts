import { StrictSchema } from 'morphism'
import { AccountResponse, Exchange } from 'shared/api/server/client/shared'

export type Account = {
  id: string
  name: string
  exchange: Exchange
  isTest: boolean
  key: string
  secret: string
}

export const AccountResponseSchema: StrictSchema<Account, AccountResponse> = {
  id: 'id',
  name: 'name',
  exchange: 'exchange',
  isTest: 'isTest',
  key: 'key',
  secret: 'secret',
}
