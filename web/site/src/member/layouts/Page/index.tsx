import { useStore } from '@annium/utils/dist/helpers'
import Divider from '@material-ui/core/Divider'
import Drawer from '@material-ui/core/Drawer'
import List from '@material-ui/core/List'
import AppsIcon from '@material-ui/icons/Apps'
import DashboardIcon from '@material-ui/icons/Dashboard'
import ExitToAppIcon from '@material-ui/icons/ExitToApp'
import PeopleIcon from '@material-ui/icons/People'
import PersonIcon from '@material-ui/icons/Person'
import cx from 'classnames'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React, { ReactNode } from 'react'
import { Logo } from 'shared/components/Logo'

import { ButtonItem } from './components/ButtonItem'
import { LinkItem } from './components/LinkItem'
import { Store } from './store'
import { useStyles } from './styles'


export const Page = observer(({ children }: { children?: ReactNode }) => {
  const store = useStore(new Store())
  const classes = useStyles()

  return (
    <div className={classes.page}>
      <Drawer
        variant="permanent"
        className={cx(classes.sidebar, {
          [classes.sidebarOpen]: store.isSidebarOpen,
          [classes.sidebarClose]: !store.isSidebarOpen,
        })}
        classes={{
          paper: cx({
            [classes.sidebarOpen]: store.isSidebarOpen,
            [classes.sidebarClose]: !store.isSidebarOpen,
          }),
        }}
      >
        <div
          className={classes.toggle}
          onClick={store.toggleSidebar}
        >
          <Logo className={classes.logo} size="medium" />
        </div>
        <Divider />
        <List>
          <LinkItem label="Dashboard" icon={<DashboardIcon />} to={routes.dashboard} />
          <LinkItem label="Apps" icon={<AppsIcon />} to={routes.apps.list} />
          <LinkItem label="Following" icon={<PeopleIcon />} to={routes.following.list} />
          <LinkItem label="Profile" icon={<PersonIcon />} to={routes.profile} />
        </List>
        <Divider />
        <List>
          <ButtonItem label="Log out" icon={<ExitToAppIcon />} onClick={store.logout} />
        </List>
      </Drawer>
      <main className={classes.content}>
        {children}
      </main>
    </div>
  )
})
