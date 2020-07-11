import { morphism, StrictSchema } from 'morphism'
import { SubscriptionResponse } from 'shared/api/server/client/subscriptionApi'

import { AccountPublicResponseSchema, PublicAccount } from './PublicAccount'

export type Subscription = {
  id: string
  master: PublicAccount
  follower: PublicAccount
}

export const SubscriptionResponseSchema: StrictSchema<Subscription, SubscriptionResponse> = {
  id: 'id',
  master: x => morphism(AccountPublicResponseSchema, x.master),
  follower: x => morphism(AccountPublicResponseSchema, x.follower),
}
