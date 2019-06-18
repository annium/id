import { createBrowserHistory } from 'history'
import React from 'react'
import { Route, Router, Switch } from 'react-router-dom'

import { App } from './App'
import { HomePage } from './pages/HomePage'
import { LoginPage } from './pages/LoginPage'
import { PersonalArea } from './PersonalArea'
import { useStyles } from './styles'

const navHistory = createBrowserHistory()


const renderNotFound = () => <h1>Not found</h1>

// tslint:disable-next-line:no-any
const renderPersonalArea = (props: any) => (
  // tslint:disable-next-line:no-unsafe-any
  <PersonalArea {...props}>
    <Switch>
      <Route path="/" exact={true} component={HomePage} />
      <Route render={renderNotFound} />
    </Switch>
  </PersonalArea>
)

// tslint:disable-next-line:no-any
const renderApp = (props: any) => (
  // tslint:disable-next-line:no-unsafe-any
  <App {...props}>
    <Switch>
      <Route path="/login" exact={true} component={LoginPage} />
      <Route render={renderPersonalArea} />
    </Switch>
  </App>
)

export const Routes = () => {
  useStyles()

  return (
    <Router history={navHistory}>
      <Route render={renderApp} />
    </Router>
  )
}
