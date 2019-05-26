import { Trans } from '@lingui/macro'
import React from 'react'
// import { RouteComponentProps } from 'react-router'

import { connect, Store } from '../../store'

import styles from './index.module.scss'


const log = console.log.bind(console, 'LoginPage')

type OwnProps = {}
type SelectorProps = Pick<Store['startup'], 'location'>

export const LoginPage = connect<OwnProps, SelectorProps>(
  ({ startup }) => ({ location: startup.location }),
  ({ location }: OwnProps & SelectorProps) => {
    log('render', location)

    return (
      <div className={styles.page}>
        <Trans>Started at {`${location.pathname}${location.search}`}</Trans>
      </div>
    )
  },
)
