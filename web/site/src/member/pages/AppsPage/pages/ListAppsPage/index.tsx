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

import { Apps } from './components/Apps'
import { MyApps } from './components/MyApps'


export const ListAppsPage = observer(() => {
  const breadcrumbs: BreadcrumbItems = { Apps: null }
  const tabs = {
    Apps: routes.apps.list,
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
        <Route path={routes.apps.list} exact={true} component={Apps} />
        <Route path={routes.apps.my} exact={true} component={MyApps} />
      </LinkTabs>
    </CentricGrid>
  )
})
