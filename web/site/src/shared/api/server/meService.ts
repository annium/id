import { meApi } from './client/meApi'
import { privateClient } from './clients'

export const meService = meApi(privateClient)
