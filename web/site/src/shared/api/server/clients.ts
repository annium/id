import { httpClientFactory } from '@annium/client-http'

import { authMiddleware, authorization } from '../id/clients'


const { protocol, hostname: host, port } = new URL(process.env.REACT_APP_API || window.location.toString())


export const publicClient = httpClientFactory({
  url: {
    protocol,
    // tslint:disable-next-line: object-literal-sort-keys
    host,
    port: parseInt(port, 10),
  },
})

export const privateClient = httpClientFactory({
  url: {
    protocol,
    // tslint:disable-next-line: object-literal-sort-keys
    host,
    port: parseInt(port, 10),
  },
  init: {
    headers: authorization({}),
  },
})

privateClient.useMiddleware(authMiddleware)
