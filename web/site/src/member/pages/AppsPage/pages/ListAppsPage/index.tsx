import Button from '@material-ui/core/Button'
import Grid from '@material-ui/core/Grid'
import Tab from '@material-ui/core/Tab'
import Tabs from '@material-ui/core/Tabs'
import TabContext from '@material-ui/lab/TabContext'
import { BreadcrumbItems, Breadcrumbs } from 'member/components/Breadcrumbs'
import { useInjection } from 'member/config/di'
import { services } from 'member/config/di/services'
import { CentricGrid } from 'member/layouts/CentricGrid'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React from 'react'
import { Route, Switch } from 'react-router-dom'
import { Link } from 'shared/components/Link'
import { IRouterStore } from 'shared/stores/RouterStore'

import { Apps } from './components/Apps'
import { MyApps } from './components/MyApps'


const TABS = [
  routes.apps.list,
  routes.apps.listMy,
]

export const ListAppsPage = observer(() => {
  const router = useInjection<IRouterStore>(services.RouterStore)

  const breadcrumbs: BreadcrumbItems = { Apps: null }
  const tab = TABS.indexOf(router.location.pathname)
  if (tab < 0)
    return null

  return (
    <CentricGrid>
      <Grid container={true} alignItems="flex-end" justify="space-between">
        <Breadcrumbs items={breadcrumbs} />
        <Link to={routes.apps.create} underline="none">
          <Button color="primary">Create</Button>
        </Link>
      </Grid>
      <TabContext value={router.location.pathname}>
        <div>
          <Tabs value={tab}>
            <LinkTab router={router} label="Apps" href={routes.apps.list} />
            <LinkTab router={router} label="My Apps" href={routes.apps.listMy} />
          </Tabs>
        </div>
        <Switch>
          <Route path={routes.apps.list} exact={true} component={Apps} />
          <Route path={routes.apps.listMy} exact={true} component={MyApps} />
        </Switch>
      </TabContext>
    </CentricGrid>
  )
})

type LinkTabProps = {
  router: IRouterStore
  label: string
  href: string
}

function LinkTab(props: LinkTabProps) {
  const { router, href } = props
  const handleClick = (event: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
    event.preventDefault()
    router.go(href)
  }

  return (
    <Tab component="a" onClick={handleClick} {...props} />
  )
}
