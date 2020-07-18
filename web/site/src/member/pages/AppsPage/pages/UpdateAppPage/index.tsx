import { Status } from '@annium/forms'
import { Form } from '@annium/forms-react'
import { ValueField } from '@annium/forms-react-mui'
import { useStore } from '@annium/utils/dist/helpers'
import Button from '@material-ui/core/Button'
import Grid from '@material-ui/core/Grid'
import Paper from '@material-ui/core/Paper'
import TextField from '@material-ui/core/TextField'
import { BreadcrumbItems, Breadcrumbs } from 'member/components/Breadcrumbs'
import { CentricGrid } from 'member/layouts/CentricGrid'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React, { useCallback, useEffect } from 'react'
import { Link } from 'shared/components/Link'
import { Loader } from 'shared/components/Loader'
import { validate } from 'shared/utils/forms'

import { Store } from './store'
import { useStyles } from './styles'
import { DataValidator } from './validators'


export const UpdateAppPage = observer(() => {
  const store = useStore(new Store())
  const form = store.form
  const state = store.state
  const classes = useStyles()

  useEffect(() => {
    store.load()
  }, [store])

  const breadcrumbs: BreadcrumbItems = {
    Apps: routes.apps.list,
    [form.name.value ? form.name.value : 'Update']: null,
  }

  const isFormSubmittable = !state.isLoading && form.hasStatus(Status.Success) &&
    form.name.value
  const handleChangeSubmit = useCallback((e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' && isFormSubmittable)
      store.update()
  }, [isFormSubmittable, store])

  useEffect(() => () => form.reset(), [form])

  return (
    <CentricGrid>
      <Breadcrumbs items={breadcrumbs} />
      <Loader direction="column" align="stretch" justify="flex-start" isLoading={state.isLoading}>
        <Form state={form} onChange={validate(DataValidator)}>
          <Paper className={classes.paper}>
            <Grid container={true} spacing={3} justify="space-between" alignItems="flex-end">
              <Grid item={true} xs={12} sm={6}>
                <ValueField field={form.name}>
                  <TextField
                    required={true}
                    label="Name"
                    autoFocus={true}
                    onKeyDown={handleChangeSubmit}
                  />
                </ValueField>
              </Grid>
              <Grid className={classes.buttons} item={true} xs={12}>
                <Button
                  disableElevation={true}
                  variant="contained"
                  color="primary"
                  disabled={!isFormSubmittable}
                  onClick={store.update}
                >
                  Update
                </Button>
                <Link to={routes.apps.my} underline="none">
                  <Button
                    disableElevation={true}
                    variant="contained"
                    color="inherit"
                  >
                    Cancel
                  </Button>
                </Link>
              </Grid>
            </Grid>
          </Paper>
        </Form>
      </Loader>
    </CentricGrid>
  )
})
