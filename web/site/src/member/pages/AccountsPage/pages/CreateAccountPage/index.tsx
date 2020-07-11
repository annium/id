import { Status } from '@annium/forms'
import { convert, Field, Form } from '@annium/forms-react'
import { CheckedField, ValueField } from '@annium/forms-react-mui'
import { FormControlLabel } from '@material-ui/core'
import Button from '@material-ui/core/Button'
import Checkbox from '@material-ui/core/Checkbox'
import Grid from '@material-ui/core/Grid'
import MenuItem from '@material-ui/core/MenuItem'
import Paper from '@material-ui/core/Paper'
import TextField from '@material-ui/core/TextField'
import { BreadcrumbItems, Breadcrumbs } from 'member/components/Breadcrumbs'
import { CentricGrid } from 'member/layouts/CentricGrid'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React, { useCallback, useEffect } from 'react'
import { Exchange } from 'shared/api/server/client/shared'
import { Link } from 'shared/components/Link'
import { Loader } from 'shared/components/Loader'
import { getPairs } from 'shared/utils/enum'
import { validate } from 'shared/utils/forms'

import { Store } from '../../store'

import { useStyles } from './styles'
import { DataValidator } from './validators'


export type CreateAccountPageProps = { store: Store }

export const CreateAccountPage = observer(({ store }: CreateAccountPageProps) => {
  const form = store.form
  const state = store.state
  const classes = useStyles()
  const breadcrumbs: BreadcrumbItems = {
    Accounts: routes.accounts.list,
    Create: null,
  }

  const isFormSubmittable = !state.isLoading && form.hasStatus(Status.Success) &&
    form.name.value && form.key.value && form.secret.value
  const handleChangeSubmit = useCallback((e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' && isFormSubmittable)
      store.create()
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
              <Grid item={true} xs={12} sm={3}>
                <Field
                  state={form.exchange}
                  fieldToState={convert.changeEventValueToNumber}
                  stateToField={convert.asIs}
                >
                  <TextField
                    select={true}
                    label="Exchange"
                  >
                    {getPairs(Exchange).map(([key, value]) => (
                      <MenuItem key={key} value={value}>{key}</MenuItem>
                    ))}
                  </TextField>
                </Field>
              </Grid>
              <Grid item={true} xs={12} sm={3}>
                <CheckedField field={form.isTest}>
                  <FormControlLabel label="Test" control={<Checkbox />} labelPlacement="end" />
                </CheckedField>
              </Grid>
              <Grid item={true} xs={12} sm={6}>
                <ValueField field={form.key}>
                  <TextField
                    required={true}
                    label="Key"
                    onKeyDown={handleChangeSubmit}
                  />
                </ValueField>
              </Grid>
              <Grid item={true} xs={12} sm={6}>
                <ValueField field={form.secret}>
                  <TextField
                    required={true}
                    label="Secret"
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
                  onClick={store.create}
                >
                  Create
                </Button>
                <Link to={routes.accounts.list} underline="none">
                  <Button
                    disableElevation={true}
                    variant="contained"
                    color="default"
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
