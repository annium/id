import { useStore } from '@annium/utils/dist/helpers'
import { BreadcrumbItems, Breadcrumbs } from 'member/components/Breadcrumbs'
import { LinkTabs } from 'member/components/LinkTabs'
import { NotFound } from 'member/components/NotFound'
import { useInjection } from 'member/config/di'
import { services } from 'member/config/di/services'
import { CentricGrid } from 'member/layouts/CentricGrid'
import { Page } from 'member/layouts/Page'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React, { useEffect } from 'react'
// import { Route } from 'react-router-dom'
import { Loader } from 'shared/components/Loader'
import { IRouterStore } from 'shared/stores/RouterStore'

import { Store } from './store'


export const UserPage = observer(() => {
  const store = useStore(new Store())
  const user = store.user
  const router = useInjection<IRouterStore>(services.RouterStore)

  const breadcrumbs: BreadcrumbItems = {
    Users: router.path(routes.apps.users.list, { app: store.appId }),
    [user.data.login || 'xxx']: null,
  }
  const tabs: Record<string, string> = user.isSuccess ? {
    User: router.path(routes.apps.users.user.view, { app: store.appId, user: user.data.id }),
  } : {}

  useEffect(() => {
    store.load()
  }, [store])

  return (
    <Page>
      <CentricGrid>
        <Breadcrumbs items={breadcrumbs} />
        <Loader direction="column" align="stretch" justify="flex-start" isLoading={user.isLoading}>
          {user.isSuccess && (
            <LinkTabs tabs={tabs}>
              {/*    <Route path={routes.apps.list} exact={true} component={Apps} />*/}
              {/*    <Route path={routes.apps.my} exact={true} component={MyApps} />*/}
            </LinkTabs>
          )}
          {user.isFailure && (
            <NotFound type="Application" id={user.data.id} />
          )}
        </Loader>
      </CentricGrid>
    </Page>
  )
})
