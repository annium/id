import { httpClientFactory } from '@annium/client-http'

import { authorization, getAuthMiddleware } from './middlewares'


const { protocol, hostname: host, port } = new URL(process.env.REACT_APP_API!)

export const publicClient = httpClientFactory({
  url: {
    protocol,
    // tslint:disable-next-line: object-literal-sort-keys
    host,
    port: parseInt(port, 10),
  },
})

export const privateBaseClient = httpClientFactory({
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

privateClient.useMiddleware(getAuthMiddleware(privateBaseClient))
