import Container from '@material-ui/core/Container'
import Typography from '@material-ui/core/Typography'
import React, { ReactNode } from 'react'

import { useStyles } from './styles'


export const Root = ({ children }: { children?: ReactNode }) => {
  const classes = useStyles()

  return (
    <main className={classes.root}>
      {children}
      <footer className={classes.footer}>
        <Container maxWidth="sm">
          <Typography variant="body1">My sticky footer can be found here.</Typography>
        </Container>
      </footer>
    </main>
  )
}
