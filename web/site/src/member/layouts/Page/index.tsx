import { useStore } from '@annium/utils/dist/helpers'
import AppBar from '@material-ui/core/AppBar'
import Container from '@material-ui/core/Container'
import Toolbar from '@material-ui/core/Toolbar'
import ExitToAppIcon from '@material-ui/icons/ExitToApp'
import { observer } from 'mobx-react-lite'
import React, { ReactNode } from 'react'
import { Logo } from 'shared/components/Logo'

import { ButtonItem } from './components/ButtonItem'
import { Store } from './store'
import { useStyles } from './styles'


type PageProps = { children: NonNullable<ReactNode> }

export const Page = observer(({ children }: PageProps) => {
  const store = useStore(new Store())
  const classes = useStyles()

  return (
    <>
      <AppBar position="fixed" className={classes.appBar}>
        <Container>
          <Toolbar>
            <Logo className={classes.logo} size="small" onClick={store.goHome} />
            <div className={classes.spaceSeparator} />
            <ButtonItem label="Log out" icon={<ExitToAppIcon />} onClick={store.logout} />
            <div className={classes.growSeparator} />
            <ButtonItem label="Log out" icon={<ExitToAppIcon />} onClick={store.logout} />
          </Toolbar>
        </Container>
      </AppBar>
      <Container className={classes.content}>
        {children}
      </Container>
    </>
  )
})
