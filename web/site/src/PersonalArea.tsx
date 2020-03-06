import React, { ReactNode, useEffect } from 'react'
import { RouteComponentProps } from 'react-router-dom'

import { Root } from './components/Root'
import { connect, Store } from './store'


const log = console.log.bind(console, 'PersonalArea')

type OwnProps = RouteComponentProps & { children?: ReactNode }
type SelectorProps = {
  auth: Store['auth']
  hasTokens: boolean
}

export const PersonalArea = connect<OwnProps, SelectorProps>(
  ({ auth }) => ({
    auth,
    hasTokens: Boolean(auth.token.data),
  }),
  ({ auth, hasTokens, history, children }: OwnProps & SelectorProps) => {
    useEffect(
      () => {
        log('update', 'ensure access')
        ensureAccess(auth, hasTokens, history)
      },
      [auth, hasTokens, history],
    )

    if (!auth.hasAccess)
      return null

    log('render')

    return (
      <Root>
        {children}
      </Root>
    )
  },
)

const ensureAccess = (
  auth: SelectorProps['auth'],
  hasTokens: boolean,
  history: OwnProps['history'],
) => {
  log('checkAccess', auth.hasAccess)
  // if has access - nothing to do
  if (auth.hasAccess)
    return

  // go to login page, if:
  // - no tokens (won't start loading)
  // - user load is finished
  if (!hasTokens || auth.user.isSuccess || auth.user.isFailure) {
    log('checkAccess', 'go to /login')
    history.replace('/login')
  }
}
