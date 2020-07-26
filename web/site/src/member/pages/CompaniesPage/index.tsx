import { Page } from 'member/layouts/Page'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React from 'react'
import { Route, Switch } from 'react-router-dom'

import { CreateCompanyPage } from './pages/CreateCompanyPage'
import { ListCompaniesPage } from './pages/ListCompaniesPage'
import { UpdateCompanyPage } from './pages/UpdateCompanyPage'
import { ViewCompanyPage } from './pages/ViewCompanyPage'


export const CompaniesPage = observer(() => (
  <Page>
    <Switch>
      <Route path={routes.apps.companies.list} exact={true} component={ListCompaniesPage} />
      <Route path={routes.apps.companies.my} exact={true} component={ListCompaniesPage} />
      <Route path={routes.apps.companies.new} exact={true} component={CreateCompanyPage} />
      <Route path={routes.apps.companies.edit} component={UpdateCompanyPage} />
      <Route path={routes.apps.companies.view} component={ViewCompanyPage} />
    </Switch>
  </Page>
))
