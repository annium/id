import { useStore } from '@annium/utils/dist/helpers'
import { BreadcrumbItems, Breadcrumbs } from 'member/components/Breadcrumbs'
import { LinkTabs } from 'member/components/LinkTabs'
import { NotFound } from 'member/components/NotFound'
import { useInjection } from 'member/config/di'
import { services } from 'member/config/di/services'
import { CentricGrid } from 'member/layouts/CentricGrid'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React, { useEffect } from 'react'
import { Loader } from 'shared/components/Loader'
import { IRouterStore } from 'shared/stores/RouterStore'

import { Store } from './store'


export const ViewAppPage = observer(() => {
  const store = useStore(new Store())
  const app = store.app
  const router = useInjection<IRouterStore>(services.RouterStore)

  const breadcrumbs: BreadcrumbItems = {
    Apps: routes.apps.list,
    [app.data.name || 'xxx']: null,
  }
  const tabs = app.isSuccess ? {
    [router.path(routes.apps.view, { app: app.data.id })]: 'App',
  } : {}

  useEffect(() => {
    store.load()
  }, [store])

  return (
    <CentricGrid>
      <Breadcrumbs items={breadcrumbs} />
      <Loader direction="column" align="stretch" justify="flex-start" isLoading={app.isLoading}>
        {app.isSuccess && (
          <LinkTabs tabs={tabs}>
            {/*    <Route path={routes.apps.list} exact={true} component={Apps} />*/}
            {/*    <Route path={routes.apps.my} exact={true} component={MyApps} />*/}
          </LinkTabs>
        )}
        {app.isFailure && (
          <NotFound type="Application" id={app.data.id} />
        )}
      </Loader>
    </CentricGrid>
  )
})
