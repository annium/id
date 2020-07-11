import { Route } from '@annium/utils/dist/components'
import { useStore } from '@annium/utils/dist/helpers'
import { Page } from 'member/layouts/Page'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React, { useEffect } from 'react'
import { Switch } from 'react-router-dom'

import { CreateFollowingPage, CreateFollowingPageProps } from './pages/CreateFollowingPage'
import { ListFollowingPage, ListFollowingPageProps } from './pages/ListFollowingPage'
import { Store } from './store'


export const FollowingPage = observer(() => {
  const store = useStore(new Store())
  // @ts-ignore
  window.store = store

  useEffect(() => {
    store.load()
  }, [store])

  return (
    <Page>
      <Switch>
        <Route<ListFollowingPageProps>
          path={routes.following.list}
          exact={true}
          component={ListFollowingPage}
          store={store}
        />
        <Route<CreateFollowingPageProps>
          path={routes.following.create}
          exact={true}
          component={CreateFollowingPage}
          store={store}
        />
      </Switch>
    </Page>
  )
})
