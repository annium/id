import { useStore } from '@annium/utils/dist/helpers'
import List from '@material-ui/core/List'
import ListItem from '@material-ui/core/ListItem'
import ListItemText from '@material-ui/core/ListItemText'
import ListSubheader from '@material-ui/core/ListSubheader'
import Paper from '@material-ui/core/Paper'
import { useInjection } from 'member/config/di'
import { services } from 'member/config/di/services'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React, { useEffect } from 'react'
import { Link } from 'shared/components/Link'
import { Loader } from 'shared/components/Loader'
import { IRouterStore } from 'shared/stores/RouterStore'

import { Store } from './store'
import { useStyles } from './styles'


export const Apps = observer(() => {
  const router = useInjection<IRouterStore>(services.RouterStore)
  const store = useStore(new Store())
  const classes = useStyles()

  useEffect(() => {
    store.load()
  }, [store])

  return (
    <Loader direction="column" align="stretch" justify="flex-start" isLoading={store.apps.isLoading}>
      <List component={Paper} subheader={<ListSubheader>Name</ListSubheader>}>
        {store.apps.data.map(app => (
          <ListItem key={app.id} button={true}>
            <Link
              className={classes.link}
              to={router.path(routes.apps.view, { app: app.id })}
              color="inherit"
              underline="none"
            >
              <ListItemText primary={app.name} />
            </Link>
          </ListItem>
        ))}
      </List>
    </Loader>
  )
})
