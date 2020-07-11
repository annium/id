import { INotificationStore } from '@annium/utils/dist/stores'
import { Container } from 'inversify'
import getDecorators from 'inversify-inject-decorators'
import { container as sharedContainer } from 'shared/config/di/container'
import { IMeStore } from 'shared/stores/MeStore'
import { IRouterStore } from 'shared/stores/RouterStore'

import { services } from './services'


export const container = new Container()
export const { lazyInject } = getDecorators(container, false)

container.bind<IMeStore>(services.MeStore)
  .toDynamicValue(() => sharedContainer.get<IMeStore>(services.MeStore))
container.bind<INotificationStore>(services.NotificationStore)
  .toDynamicValue(() => sharedContainer.get<INotificationStore>(services.NotificationStore))
container.bind<IRouterStore>(services.RouterStore)
  .toDynamicValue(() => sharedContainer.get<IRouterStore>(services.RouterStore))
