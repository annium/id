import { subscriptionApi } from './client/subscriptionApi'
import { privateClient } from './clients'

export const subscriptionService = subscriptionApi(privateClient)
