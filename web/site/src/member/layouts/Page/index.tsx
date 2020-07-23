import { useStore } from '@annium/utils/dist/helpers'
import AppBar from '@material-ui/core/AppBar'
import Container from '@material-ui/core/Container'
import Toolbar from '@material-ui/core/Toolbar'
import AppsIcon from '@material-ui/icons/Apps'
import BusinessCenterIcon from '@material-ui/icons/BusinessCenter'
import ExitToAppIcon from '@material-ui/icons/ExitToApp'
import { HeaderAutocomplete } from 'member/layouts/Page/components/HeaderAutocomplete'
import { LinkItem } from 'member/layouts/Page/components/LinkItem'
import { App } from 'member/models/App'
import { Company } from 'member/models/Company'
import { observer } from 'mobx-react-lite'
import React, { ReactNode } from 'react'
import { Logo } from 'shared/components/Logo'

import { ButtonItem } from './components/ButtonItem'
import { Store } from './store'
import { useStyles } from './styles'


type PageProps = { children: NonNullable<ReactNode> }

export const Page = observer(({ children }: PageProps) => {
  const store = useStore(new Store())
  const appStore = store.app
  const companyStore = store.company
  const classes = useStyles()

  return (
    <>
      <AppBar position="fixed" className={classes.appBar}>
        <Container>
          <Toolbar>
            <Logo className={classes.logo} size="small" />
            <div className={classes.spaceSeparator} />
            <LinkItem label="Apps" icon={<AppsIcon />} to="/member" />
            <div className={classes.spaceSeparator} />
            <LinkItem label="Companies" icon={<BusinessCenterIcon />} to="/member/companies" />
            <div className={classes.growSeparator} />
            <HeaderAutocomplete<App>
              className={classes.autoComplete}
              label="App"
              data={appStore.items.data}
              value={appStore.current}
              set={appStore.set}
              search={appStore.load}
            />
            <div className={classes.spaceSeparator} />
            <HeaderAutocomplete<Company>
              className={classes.autoComplete}
              label="Company"
              data={companyStore.items.data}
              value={companyStore.current}
              set={companyStore.set}
              search={companyStore.load}
            />
            <div className={classes.spaceSeparator} />
            <ButtonItem icon={<ExitToAppIcon />} onClick={store.logout} />
          </Toolbar>
        </Container>
      </AppBar>
      <Container className={classes.content}>
        {children}
      </Container>
    </>
  )
})

