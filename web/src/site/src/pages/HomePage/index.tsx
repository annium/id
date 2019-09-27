import { Trans } from '@lingui/macro'
import Container from '@material-ui/core/Container'
import React from 'react'
import { RouteComponentProps } from 'react-router'

import { connect, Store } from '../../store'

import { useStyles } from './styles'


type OwnProps = RouteComponentProps
type SelectorProps = {
  user: Store['auth']['user']['data']
}

export const HomePage = connect<OwnProps, SelectorProps>(
  ({ auth }) => ({ user: auth.user.data }),
  ({ user }: OwnProps & SelectorProps) => {
    const classes = useStyles()

    return (
      <Container className={classes.page} component="main" maxWidth="xs">
        <div className={classes.container}>
          <Trans>Hello, {user!.login}</Trans>
        </div>
      </Container>
    )
  },
)
