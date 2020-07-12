import { Status } from '@annium/forms'
import { Form } from '@annium/forms-react'
import { ValueField } from '@annium/forms-react-mui'
import { useStore } from '@annium/utils/dist/helpers'
import { Typography } from '@material-ui/core'
import Button from '@material-ui/core/Button'
import Grid from '@material-ui/core/Grid'
import Paper from '@material-ui/core/Paper'
import TextField from '@material-ui/core/TextField'
import { observer } from 'mobx-react-lite'
import React, { useCallback, useEffect } from 'react'
import { Loader } from 'shared/components/Loader'
import { validate } from 'shared/utils/forms'

import { Store } from './store'
import { useStyles } from './styles'
import { DataValidator } from './validators'


export const AccountForm = observer(() => {
  const store = useStore(new Store())
  const form = store.form
  const state = store.state
  const classes = useStyles()

  useEffect(() => {
    store.load()
  }, [store])

  const isFormSubmittable = !state.isLoading && form.hasStatus(Status.Success) && form.hasBeenTouched
  const handleChangeSubmit = useCallback(async (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' && isFormSubmittable)
      await store.save()
  }, [isFormSubmittable, store])

  return (
    <>
      <Typography variant="body1" className={classes.title}>
        Account
      </Typography>
      <Loader direction="column" align="stretch" justify="flex-start" isLoading={state.isLoading}>
        <Form state={form} onChange={validate(DataValidator)}>
          <Paper className={classes.paper}>
            <Grid container={true} spacing={2} justify="space-between" alignItems="flex-end">
              <Grid item={true} xs={12} sm={6}>
                <ValueField field={form.login}>
                  <TextField
                    required={true}
                    label="Name"
                    autoFocus={true}
                    onKeyDown={handleChangeSubmit}
                    fullWidth={true}
                  />
                </ValueField>
              </Grid>
              <Grid item={true} xs={12} sm={6}>
                <ValueField field={form.email}>
                  <TextField
                    required={true}
                    label="Email"
                    onKeyDown={handleChangeSubmit}
                    fullWidth={true}
                  />
                </ValueField>
              </Grid>
              <Grid className={classes.buttons} item={true} xs={12}>
                <Button
                  disableElevation={true}
                  variant="contained"
                  color="primary"
                  disabled={!isFormSubmittable}
                  onClick={store.save}
                >
                  Save
                </Button>
              </Grid>
            </Grid>
          </Paper>
        </Form>
      </Loader>
    </>
  )
})
