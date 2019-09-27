import React, { ReactNode, useEffect } from 'react'
import { RouteComponentProps } from 'react-router-dom'

import { Loader } from './components/Loader'
import { authActions } from './data/auth'
import { useNotifications } from './notifications'
import { connect, Store } from './store'


const log = console.log.bind(console, 'App')

type OwnProps = RouteComponentProps & { children?: ReactNode }
type SelectorProps = {
  startup: Store['startup']
  isLoading: boolean
  hasTokens: boolean
}

export const App = connect<OwnProps, SelectorProps>(
  ({ auth, startup }) => ({
    startup,
    isLoading: auth.user.isLoading,
    hasTokens: Boolean(auth.token.data),
  }),
  function App(props: OwnProps & SelectorProps) {
    const { location, children, startup, isLoading, hasTokens } = props

    const { error } = useNotifications()

    useEffect(
      () => {
        startup.location = location
        if (hasTokens) {
          authActions.load({}).then(response => {
            if (response.isFailure)
              error(response.plainErrors[0])
          })
          log('mount', 'load user')
        }
      },
      // eslint-disable-next-line
      [],
    )

    if (isLoading)
      return <Loader isLoading={isLoading} />

    return children as JSX.Element
  },
)
