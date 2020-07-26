import Button from '@material-ui/core/Button'
import Grid from '@material-ui/core/Grid'
import { BreadcrumbItems, Breadcrumbs } from 'member/components/Breadcrumbs'
import { LinkTabs } from 'member/components/LinkTabs'
import { CentricGrid } from 'member/layouts/CentricGrid'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React from 'react'
import { Route } from 'react-router-dom'
import { Link } from 'shared/components/Link'

import { Companies } from './components/Companies'
import { MyCompanies } from './components/MyCompanies'


export const ListCompaniesPage = observer(() => {
  const breadcrumbs: BreadcrumbItems = { Apps: null }
  const tabs = {
    Companies: routes.apps.companies.list,
    'My Apps': routes.apps.my,
  }

  return (
    <CentricGrid>
      <Grid container={true} alignItems="flex-end" justify="space-between">
        <Breadcrumbs items={breadcrumbs} />
        <Link to={routes.apps.new} underline="none">
          <Button color="primary">Create</Button>
        </Link>
      </Grid>
      <LinkTabs tabs={tabs}>
        <Route path={routes.apps.list} exact={true} component={Companies} />
        <Route path={routes.apps.my} exact={true} component={MyCompanies} />
      </LinkTabs>
    </CentricGrid>
  )
})
