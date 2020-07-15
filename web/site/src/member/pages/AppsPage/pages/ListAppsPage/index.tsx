import Button from '@material-ui/core/Button'
import Grid from '@material-ui/core/Grid'
import IconButton from '@material-ui/core/IconButton'
import Paper from '@material-ui/core/Paper'
import Tab from '@material-ui/core/Tab'
import Table from '@material-ui/core/Table'
import TableBody from '@material-ui/core/TableBody'
import TableCell from '@material-ui/core/TableCell'
import TableContainer from '@material-ui/core/TableContainer'
import TableHead from '@material-ui/core/TableHead'
import TableRow from '@material-ui/core/TableRow'
import Tabs from '@material-ui/core/Tabs'
import DeleteIcon from '@material-ui/icons/Delete'
import EditIcon from '@material-ui/icons/Edit'
import TabContext from '@material-ui/lab/TabContext'
import TabPanel from '@material-ui/lab/TabPanel'
import { BreadcrumbItems, Breadcrumbs } from 'member/components/Breadcrumbs'
import { ConfirmationDialog } from 'member/components/ConfirmationDialog'
import { useInjection } from 'member/config/di'
import { services } from 'member/config/di/services'
import { CentricGrid } from 'member/layouts/CentricGrid'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React, { useEffect, useState } from 'react'
import { Link } from 'shared/components/Link'
import { Loader } from 'shared/components/Loader'
import { IRouterStore } from 'shared/stores/RouterStore'

import { Store } from '../../store'

import { useStyles } from './styles'


export type ListAppsPageProps = { store: Store }

export enum PageTab {
  apps = 'apps',
  myApps = 'my-apps',
}

export const ListAppsPage = observer(({ store }: ListAppsPageProps) => {
  const router = useInjection<IRouterStore>(services.RouterStore)
  const classes = useStyles()

  const breadcrumbs: BreadcrumbItems = { Apps: null }
  const [tab, setState] = useState<PageTab>(PageTab.apps)
  const setTab = (_: unknown, val: PageTab) => setState(val)

  useEffect(() => {
    store.load()
    store.loadMy()
  }, [store])

  return (
    <CentricGrid>
      <Grid container={true} alignItems="flex-end" justify="space-between">
        <Breadcrumbs items={breadcrumbs} />
        <Link to={routes.apps.create} underline="none">
          <Button color="primary">Create</Button>
        </Link>
      </Grid>
      <TabContext value={tab}>
        <div>
          <Tabs value={tab} onChange={setTab}>
            <Tab label="Apps" value={PageTab.apps} />
            <Tab label="My Apps" value={PageTab.myApps} />
          </Tabs>
        </div>
        <TabPanel value={PageTab.apps}>
          <Loader direction="column" align="stretch" justify="flex-start" isLoading={store.apps.isLoading}>
            <TableContainer component={Paper}>
              <Table aria-label="keys table">
                <TableHead>
                  <TableRow>
                    <TableCell>Name</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {store.apps.data.map(app => (
                    <TableRow key={app.id}>
                      <TableCell component="th" scope="row">{app.name}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          </Loader>
        </TabPanel>
        <TabPanel value={PageTab.myApps}>
          <Loader direction="column" align="stretch" justify="flex-start" isLoading={store.myApps.isLoading}>
            <TableContainer component={Paper}>
              <Table aria-label="keys table">
                <TableHead>
                  <TableRow>
                    <TableCell>Name</TableCell>
                    <TableCell>&nbsp;</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {store.myApps.data.map(app => (
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
          </Loader>
        </TabPanel>
      </TabContext>
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
