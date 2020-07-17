import { Page } from 'member/layouts/Page'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React from 'react'
import { Route, Switch } from 'react-router-dom'

import { CreateAppPage } from './pages/CreateAppPage'
import { ListAppsPage } from './pages/ListAppsPage'
import { UpdateAppPage } from './pages/UpdateAppPage'


export const AppsPage = observer(() => (
  <Page>
    <Switch>
      <Route path={routes.apps.list} exact={true} component={ListAppsPage} />
      <Route path={routes.apps.listMy} exact={true} component={ListAppsPage} />
      <Route path={routes.apps.create} exact={true} component={CreateAppPage} />
      <Route path={routes.apps.update} component={UpdateAppPage} />
    </Switch>
  </Page>
))
