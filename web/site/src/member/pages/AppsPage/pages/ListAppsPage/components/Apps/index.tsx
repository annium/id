import { useStore } from '@annium/utils/dist/helpers'
import Paper from '@material-ui/core/Paper'
import Table from '@material-ui/core/Table'
import TableBody from '@material-ui/core/TableBody'
import TableCell from '@material-ui/core/TableCell'
import TableContainer from '@material-ui/core/TableContainer'
import TableHead from '@material-ui/core/TableHead'
import TableRow from '@material-ui/core/TableRow'
import { observer } from 'mobx-react-lite'
import React, { useEffect } from 'react'
import { Loader } from 'shared/components/Loader'

import { Store } from './store'


export const Apps = observer(() => {
  const store = useStore(new Store())

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
  )
})
