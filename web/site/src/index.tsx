// tslint:disable-next-line: no-import-side-effect
import 'mobx-react-lite/batchingForReactDom'
// tslint:disable-next-line: no-import-side-effect
import 'reflect-metadata'

// tslint:disable-next-line: ordered-imports
import { history } from '@annium/utils/dist/stores/RouterStore'
import React from 'react'
import ReactDOM from 'react-dom'
import { Router, Switch } from 'react-router-dom'

// import { App as MemberApp } from './member'
import { App as PublicApp } from './public'

ReactDOM.render(
  (
    <Router history={history}>
      <Switch>
        {/*<Route path="/member" component={MemberApp} />*/}
        <PublicApp />
      </Switch>
    </Router>
  ),
  document.getElementById('root'),
)
