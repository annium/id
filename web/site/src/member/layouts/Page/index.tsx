import { useStore } from '@annium/utils/dist/helpers'
import { IconButton } from '@material-ui/core'
import AppBar from '@material-ui/core/AppBar'
import Container from '@material-ui/core/Container'
import Toolbar from '@material-ui/core/Toolbar'
import ExitToAppIcon from '@material-ui/icons/ExitToApp'
import { observer } from 'mobx-react-lite'
import React, { ReactNode } from 'react'
import { Logo } from 'shared/components/Logo'

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
            <Logo className={classes.logo} size="small" />
            <IconButton
              edge="end"
              aria-label="Log out"
              aria-haspopup="true"
              onClick={store.logout}
              color="inherit"
            >
              <ExitToAppIcon />
            </IconButton>
          </Toolbar>
        </Container>
      </AppBar>
      <Container className={classes.content}>
        {children}
      </Container>
    </>
  )
})
