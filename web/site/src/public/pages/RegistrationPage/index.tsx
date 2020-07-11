import { useStore } from '@annium/utils/dist/helpers'
import { observer } from 'mobx-react-lite'
import { MinimalPage } from 'public/layouts/MinimalPage'
import React from 'react'

import { RegistrationForm } from './RegistrationForm'
import { RegistrationSuccess } from './RegistrationSuccess'
import { Store } from './store'


export const RegisterPage = observer(() => {
  const store = useStore(new Store())

  const view = store.state.isSuccess
    ? <RegistrationSuccess />
    : <RegistrationForm store={store} />

  return (
    <MinimalPage title="registration">
      {view}
    </MinimalPage>
  )
})
