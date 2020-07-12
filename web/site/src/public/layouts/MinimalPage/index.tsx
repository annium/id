import Container from '@material-ui/core/Container'
import Typography from '@material-ui/core/Typography'
import React, { ReactNode } from 'react'
import { Logo } from 'shared/components/Logo'

import { useStyles } from './styles'


type Props = {
  title: string
  children: ReactNode
}

export const MinimalPage = ({ title, children }: Props) => {
  const classes = useStyles()

  return (
    <Container className={classes.page} component="main" maxWidth="xs">
      <div className={classes.container}>
        <Logo className={classes.avatar} size="medium" />
        <Typography component="h1" variant="h5">
          Annium ID {title}
        </Typography>
        {children}
      </div>
      <div className={classes.credentials}>
        <Typography variant="body2" color="textSecondary" align="center">
          Built with love by the <a className={classes.link} href="https://annium.com/"> Annium</a> team.
        </Typography>
      </div>
    </ Container>
  )
}
