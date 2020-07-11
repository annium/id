import { Route } from '@annium/utils/dist/components'
import { useStore } from '@annium/utils/dist/helpers'
import { Page } from 'member/layouts/Page'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React, { useEffect } from 'react'
import { Switch } from 'react-router-dom'

import { CreateAccountPage, CreateAccountPageProps } from './pages/CreateAccountPage'
import { ListAccountsPage, ListAccountsPageProps } from './pages/ListAccountsPage'
import { UpdateAccountPage, UpdateAccountPageProps } from './pages/UpdateAccountPage'
import { Store } from './store'


export const AccountsPage = observer(() => {
  const store = useStore(new Store())

  useEffect(() => {
    store.load()
  }, [store])

  return (
    <Page>
      <Switch>
        <Route<ListAccountsPageProps>
          path={routes.accounts.list}
          exact={true}
          component={ListAccountsPage}
          store={store}
        />
        <Route<CreateAccountPageProps>
          path={routes.accounts.create}
          exact={true}
          component={CreateAccountPage}
          store={store}
        />
        <Route<UpdateAccountPageProps>
          path={routes.accounts.update}
          component={UpdateAccountPage}
          store={store}
        />
      </Switch>
    </Page>
  )
})
