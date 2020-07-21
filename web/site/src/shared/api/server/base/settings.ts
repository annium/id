const { protocol, hostname: host, port } = new URL(process.env.REACT_APP_API!)

export const api = {
  protocol,
  host,
  port,
}

export const app = {
  key: process.env.REACT_APP_APP_ID!,
}
