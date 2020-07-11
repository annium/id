import React from 'react'
import { Route, Switch } from 'react-router-dom'

import { Area } from './components/Area'
import { ConfirmEmailPage } from './pages/ConfirmEmailPage'
import { ExternalLoginPage } from './pages/ExternalLoginPage'
import { HomePage } from './pages/HomePage'
import { LoginPage } from './pages/LoginPage'
import { NotFoundPage } from './pages/NotFoundPage'
import { RegisterPage } from './pages/RegistrationPage'
import { RestoreAccessPage } from './pages/RestoreAccessPage'
import { Routes } from './routes'


export const Module = () => (
  <Switch>
    <Route path={Routes.confirmEmail} exact={true} component={area(ConfirmEmailPage)} />
    <Route path={Routes.externalLogin} exact={true} component={area(ExternalLoginPage)} />
    <Route path={Routes.home} exact={true} component={area(HomePage)} />
    <Route path={Routes.login} exact={true} component={area(LoginPage)} />
    <Route path={Routes.register} exact={true} component={area(RegisterPage)} />
    <Route path={Routes.restoreAccess} exact={true} component={area(RestoreAccessPage)} />
    <Route component={NotFoundPage} />
  </Switch>
)

const area = (Component: React.FunctionComponent) => () => (<Area><Component /></Area>)
