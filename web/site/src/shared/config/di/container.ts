import { INotificationStore, NotificationStore } from '@annium/utils/dist/stores'
import { Container } from 'inversify'
import { IMeStore, MeStore } from 'shared/stores/MeStore'
import { IRouterStore, RouterStore } from 'shared/stores/RouterStore'

import { servicesFactory } from './servicesFactory'

const services = servicesFactory.build()
const container = new Container()

container.bind<IMeStore>(services.MeStore)
  .to(MeStore).inSingletonScope()
container.bind<INotificationStore>(services.NotificationStore)
  .toConstantValue(new NotificationStore(3))
container.bind<IRouterStore>(services.RouterStore)
  .to(RouterStore).inSingletonScope()

export { container }
