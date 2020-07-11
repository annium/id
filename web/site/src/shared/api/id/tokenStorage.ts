import { storage } from '@annium/utils/dist/helpers'

import { TokensResponse } from './client/shared'

const storageKeys = {
  token: 'token',
}

export const tokenStorage = {
  get: (): TokensResponse | null => storage.get<TokensResponse>(storageKeys.token),
  set: (tokens: TokensResponse) => storage.set<TokensResponse>(storageKeys.token, tokens),
  clear: () => storage.remove(storageKeys.token),
}
