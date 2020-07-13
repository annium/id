import { Route } from '@annium/utils/dist/components'
import { useStore } from '@annium/utils/dist/helpers'
import { Page } from 'member/layouts/Page'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React from 'react'
import { Switch } from 'react-router-dom'

import { CreateAppPage, CreateAppPageProps } from './pages/CreateAppPage'
import { ListAppsPage, ListAppsPageProps } from './pages/ListAppsPage'
// import { UpdateAccountPage, UpdateAccountPageProps } from './pages/UpdateAccountPage'
import { Store } from './store'


export const AppsPage = observer(() => {
  const store = useStore(new Store())

  return (
    <Page>
      <Switch>
        <Route<ListAppsPageProps>
          path={routes.apps.list}
          exact={true}
          component={ListAppsPage}
          store={store}
        />
        <Route<CreateAppPageProps>
          path={routes.apps.create}
          exact={true}
          component={CreateAppPage}
          store={store}
        />
        {/*<Route<UpdateAccountPageProps>*/}
        {/*  path={routes.accounts.update}*/}
        {/*  component={UpdateAccountPage}*/}
        {/*  store={store}*/}
        {/*/>*/}
      </Switch>
    </Page>
  )
})
