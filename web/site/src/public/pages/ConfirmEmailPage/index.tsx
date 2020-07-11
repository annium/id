import { useStore } from '@annium/utils/dist/helpers'
import { observer } from 'mobx-react-lite'
import { MinimalPage } from 'public/layouts/MinimalPage'
import React, { useEffect } from 'react'
import { Loader } from 'shared/components/Loader'

import { Failure } from './Failure'
import { InvalidLink } from './InvalidLink'
import { ConfirmationStatus, Store } from './store'
import { Success } from './Success'


export const ConfirmEmailPage = observer(() => {
  const store = useStore(new Store())

  useEffect(() => {
    if (!store.me.hasAccess)
      store.init()
  }, [store])

  return (
    <MinimalPage title="Email confirmation">
      <Loader direction="column" align="stretch" justify="flex-start" isLoading={store.state.isLoading}>
        {(() => {
          if (store.state.data === ConfirmationStatus.InvalidLink)
            return <InvalidLink />

          if (store.state.data === ConfirmationStatus.Failure)
            return <Failure reason={store.state.plainErrors.join(', ')} />

          if (store.state.data === ConfirmationStatus.Success)
            return <Success />

          return null
        })()}
      </Loader>
    </MinimalPage>
  )
})
