import { IconButton } from '@material-ui/core'
import Button from '@material-ui/core/Button'
import Grid from '@material-ui/core/Grid'
import Paper from '@material-ui/core/Paper'
import Table from '@material-ui/core/Table'
import TableBody from '@material-ui/core/TableBody'
import TableCell from '@material-ui/core/TableCell'
import TableContainer from '@material-ui/core/TableContainer'
import TableHead from '@material-ui/core/TableHead'
import TableRow from '@material-ui/core/TableRow'
import DeleteIcon from '@material-ui/icons/Delete'
import EditIcon from '@material-ui/icons/Edit'
import { BreadcrumbItems, Breadcrumbs } from 'member/components/Breadcrumbs'
import { ConfirmationDialog } from 'member/components/ConfirmationDialog'
import { useInjection } from 'member/config/di'
import { services } from 'member/config/di/services'
import { CentricGrid } from 'member/layouts/CentricGrid'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React from 'react'
import { Exchange } from 'shared/api/server/client/shared'
import { Link } from 'shared/components/Link'
import { Loader } from 'shared/components/Loader'
import { IRouterStore } from 'shared/stores/RouterStore'

import { Store } from '../../store'

import { useStyles } from './styles'


export type ListAccountsPageProps = { store: Store }

export const ListAccountsPage = observer(({ store }: ListAccountsPageProps) => {
  const router = useInjection<IRouterStore>(services.RouterStore)
  const classes = useStyles()
  const breadcrumbs: BreadcrumbItems = { Accounts: null }

  return (
    <CentricGrid>
      <Grid container={true} alignItems="flex-end" justify="space-between">
        <Breadcrumbs items={breadcrumbs} />
        <Link to={routes.accounts.create} underline="none">
          <Button color="primary">Create</Button>
        </Link>
      </Grid>
      <Loader direction="column" align="stretch" justify="flex-start" isLoading={store.accounts.isLoading}>
        <TableContainer component={Paper}>
          <Table aria-label="keys table">
            <TableHead>
              <TableRow>
                <TableCell>Name</TableCell>
                <TableCell>Exchange</TableCell>
                <TableCell>Type</TableCell>
                <TableCell>Key</TableCell>
                <TableCell>Secret</TableCell>
                <TableCell>{''}</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {store.accounts.data.map(account => (
                <TableRow key={account.id}>
                  <TableCell component="th" scope="row">{account.name}</TableCell>
                  <TableCell component="th" scope="row">{Exchange[account.exchange]}</TableCell>
                  <TableCell>{account.isTest ? 'TEST' : 'REAL'}</TableCell>
                  <TableCell>{account.key}</TableCell>
                  <TableCell>{account.secret}</TableCell>
                  <TableCell className={classes.itemActions}>
                    <Link to={router.path(routes.accounts.update, { id: account.id })}>
                      <IconButton color="default" component="button">
                        <EditIcon />
                      </IconButton>
                    </Link>
                    <IconButton color="default" component="button" onClick={store.setDeleteCandidate(account.id)}>
                      <DeleteIcon />
                    </IconButton>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Loader>
      <ConfirmationDialog
        isOpen={Boolean(store.deleteCandidate.get())}
        title="Delete account?"
        onConfirm={store.delete}
        onCancel={store.setDeleteCandidate(null)}
      >
        Removing account is irreversible.
      </ConfirmationDialog>
    </CentricGrid>
  )
})
