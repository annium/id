import { di } from '@annium/utils'

export const servicesFactory = di.serviceRegistryFactory()
  .add('MeStore')
  .add('NotificationStore')
  .add('RouterStore')
