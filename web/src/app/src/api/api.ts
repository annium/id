import { factory } from '@annium/server-http'

import { context } from '../context'

const { protocol, hostname: host, port } = new URL(process.env.REACT_APP_API_URL || window.location.toString())

export const publicApi = factory({
  url: {
    protocol,
    // tslint:disable-next-line: object-literal-sort-keys
    host,
    port: parseInt(port, 10),
  },
})

export const privateApi = factory({
  url: {
    protocol,
    // tslint:disable-next-line: object-literal-sort-keys
    host,
    port: parseInt(port, 10),
  },
  init: {
    headers: {
      get Authorization() {
        return `Bearer ${context.getState().auth.token.data!.accessToken}`
      },
    },
  },
})
