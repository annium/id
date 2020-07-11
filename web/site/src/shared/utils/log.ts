export const getLogFactory = (app: string) => (component: string) => console.log.bind(console, app, component)
export const getLog = getLogFactory('shared')
