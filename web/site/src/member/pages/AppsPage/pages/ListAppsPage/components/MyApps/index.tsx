import { useStore } from '@annium/utils/dist/helpers'
import IconButton from '@material-ui/core/IconButton'
import Paper from '@material-ui/core/Paper'
import Table from '@material-ui/core/Table'
import TableBody from '@material-ui/core/TableBody'
import TableCell from '@material-ui/core/TableCell'
import TableContainer from '@material-ui/core/TableContainer'
import TableHead from '@material-ui/core/TableHead'
import TableRow from '@material-ui/core/TableRow'
import DeleteIcon from '@material-ui/icons/Delete'
import EditIcon from '@material-ui/icons/Edit'
import { ConfirmationDialog } from 'member/components/ConfirmationDialog'
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


export const MyApps = observer(() => {
  const store = useStore(new Store())
  const router = useInjection<IRouterStore>(services.RouterStore)
  const classes = useStyles()

  useEffect(() => {
    store.load()
  }, [store])

  return (
    <Loader direction="column" align="stretch" justify="flex-start" isLoading={store.apps.isLoading}>
      <TableContainer component={Paper}>
        <Table aria-label="keys table">
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>&nbsp;</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {store.apps.data.map(app => (
              <TableRow key={app.id}>
                <TableCell scope="row">{app.name}</TableCell>
                <TableCell className={classes.itemActions}>
                  <Link to={router.path(routes.apps.update, { id: app.id })}>
                    <IconButton component="button" size="small">
                      <EditIcon />
                    </IconButton>
                  </Link>
                  <IconButton component="button" size="small" onClick={store.setDeleteCandidate(app.id)}>
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <ConfirmationDialog
        isOpen={Boolean(store.deleteCandidate.get())}
        title="Delete account?"
        onConfirm={store.delete}
        onCancel={store.setDeleteCandidate(null)}
      >
        Removing account is irreversible.
      </ConfirmationDialog>
    </Loader>
  )
})
