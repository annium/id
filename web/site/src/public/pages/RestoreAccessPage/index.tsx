import { useStore } from '@annium/utils/dist/helpers'
import { observer } from 'mobx-react-lite'
import { MinimalPage } from 'public/layouts/MinimalPage'
import React from 'react'

import { RestoreForm } from './RestoreForm'
import { RestoreSuccess } from './RestoreSuccess'
import { Store } from './store'


export const RestoreAccessPage = observer(() => {
  const store = useStore(new Store())

  const view = store.state.isSuccess
    ? <RestoreSuccess email={store.data.email} />
    : <RestoreForm store={store} />

  return (
    <MinimalPage title="access restore">
      {view}
    </MinimalPage>
  )
})
