import React from 'react'
import { Route, Switch } from 'react-router-dom'

import { Area } from './components/Area'
import { AppsPage } from './pages/AppsPage'
import { DashboardPage } from './pages/DashboardPage'
import { ProfilePage } from './pages/ProfilePage'
import { routes } from './routes'


export const Module = () => (
  <Area>
    <Switch>
      <Route path={routes.apps.list} component={AppsPage} />
      <Route path={routes.dashboard} exact={true} component={DashboardPage} />
      <Route path={routes.profile} exact={true} component={ProfilePage} />
    </Switch>
  </Area>
)
