import { di } from '@annium/utils'

import { container } from './container'
import { services } from './services'


export const {
  containerContext,
  ContainerProvider,
  useInjection,
} = di.getContainerContext(container, services, 'member')
