import React, { ReactNode, useEffect } from 'react'
import { RouteComponentProps } from 'react-router-dom'

import { Loader } from './components/Loader'
import { authActions } from './data/auth'
import { connect, Store } from './store'


const log = console.log.bind(console, 'App')

type OwnProps = RouteComponentProps & { children?: ReactNode }
type SelectorProps = Pick<Store, 'startup'>
  & {
    isLoading: boolean
  }

export const App = connect<OwnProps, SelectorProps>(
  ({ auth, startup }) => ({ startup, isLoading: auth.user.isLoading }),
  function App(props: OwnProps & SelectorProps) {
    const { location, children, startup, isLoading } = props

    useEffect(
      () => {
        startup.location = location
        log('mount', 'load user')
        authActions.load({})
      },
      // eslint-disable-next-line
      [],
    )

    if (isLoading)
      return <Loader isLoading={isLoading} />

    return children as JSX.Element
  },
)
