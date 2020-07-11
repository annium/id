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
import { BreadcrumbItems, Breadcrumbs } from 'member/components/Breadcrumbs'
import { ConfirmationDialog } from 'member/components/ConfirmationDialog'
import { CentricGrid } from 'member/layouts/CentricGrid'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React from 'react'
import { Exchange } from 'shared/api/server/client/shared'
import { Link } from 'shared/components/Link'
import { Loader } from 'shared/components/Loader'

import { Store } from '../../store'

import { useStyles } from './styles'


export type ListFollowingPageProps = { store: Store }

export const ListFollowingPage = observer(({ store }: ListFollowingPageProps) => {
  const classes = useStyles()
  const breadcrumbs: BreadcrumbItems = { Following: null }

  return (
    <CentricGrid>
      <Grid container={true} alignItems="flex-end" justify="space-between">
        <Breadcrumbs items={breadcrumbs} />
        <Link to={routes.following.create} underline="none">
          <Button color="primary">Create</Button>
        </Link>
      </Grid>
      <Loader direction="column" align="stretch" justify="flex-start" isLoading={store.subscriptions.isLoading}>
        <TableContainer component={Paper}>
          <Table aria-label="keys table">
            <TableHead>
              <TableRow>
                <TableCell>Account</TableCell>
                <TableCell>Target</TableCell>
                <TableCell>{''}</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {store.subscriptions.data.map(({ id, master, follower }) => (
                <TableRow key={id}>
                  <TableCell component="th" scope="row">
                    {follower.name} ({Exchange[follower.exchange]}, {follower.isTest ? 'TEST' : 'REAL'})
                  </TableCell>
                  <TableCell component="th" scope="row">
                    {master.user.login}, {master.name} ({Exchange[master.exchange]}, {master.isTest ? 'TEST' : 'REAL'})
                  </TableCell>
                  <TableCell className={classes.itemActions}>
                    <IconButton color="default" component="button" onClick={store.setDeleteCandidate(id)}>
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
