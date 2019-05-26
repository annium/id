import React, { ReactNode, useEffect } from 'react'
import { RouteComponentProps } from 'react-router-dom'

import { Root } from './components/Root'
import { connect, Store } from './store'


const log = console.log.bind(console, 'PersonalArea')

type OwnProps = RouteComponentProps & { children?: ReactNode }
type SelectorProps = Pick<Store, 'auth'>

export const PersonalArea = connect<OwnProps, SelectorProps>(
  ({ auth }) => ({ auth }),
  ({ auth, history, children }: OwnProps & SelectorProps) => {
    useEffect(
      () => {
        log('update', 'ensure access')
        ensureAccess(auth, history)
      },
      [auth, history],
    )

    if (!auth.access)
      return null

    log('render')

    return (
      <Root>
        {children}
      </Root>
    )
  },
)

const ensureAccess = (auth: SelectorProps['auth'], history: OwnProps['history']) => {
  log('checkAccess', auth.access)
  if ((auth.user.isSuccess || auth.user.isFailure) && !auth.access)
    history.replace('/login')
}

