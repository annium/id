import { Trans } from '@lingui/macro'
import Container from '@material-ui/core/Container'
import { useInjection } from 'member/config/di'
import { services } from 'member/config/di/services'
import { observer } from 'mobx-react-lite'
import React from 'react'
import { IMeStore } from 'shared/stores/MeStore'

import { Page } from '../../layouts/Page'

import { useStyles } from './styles'


export const DashboardPage = observer(() => {
  const me = useInjection<IMeStore>(services.MeStore)
  const classes = useStyles()

  return (
    <Page>
      <Container className={classes.container} component="main" maxWidth="xs">
        <Trans>Hello, {me.user!.login}</Trans>
      </Container>
    </Page>
  )
})
