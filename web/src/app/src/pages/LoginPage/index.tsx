import { Trans } from '@lingui/macro'
import React from 'react'

import { connect, Store } from '../../store'

import { useStyles } from './styles'


const log = console.log.bind(console, 'LoginPage')

type OwnProps = {}
type SelectorProps = Pick<Store['startup'], 'location'>

export const LoginPage = connect<OwnProps, SelectorProps>(
  ({ startup }) => ({ location: startup.location }),
  ({ location }: OwnProps & SelectorProps) => {
    const classes = useStyles()
    log('render', location)

    return (
      <div className={classes.page}>
        <Trans>Started at {`${location.pathname}${location.search}`}</Trans>
      </div>
    )
  },
)
