import Container from '@material-ui/core/Container'
import Typography from '@material-ui/core/Typography'
import React, { ReactNode } from 'react'
import { Link } from 'shared/components/Link'
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
          Crypted.One {title}
        </Typography>
        {children}
      </div>
      <div className={classes.credentials}>
        <Typography variant="body2" color="textSecondary" align="center">
          Built with love by the <Link color="inherit" to="https://crypted.one/"> Crypted</Link> team.
        </Typography>
      </div>
    </ Container>
  )
}
