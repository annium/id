import { Status } from '@annium/forms'
import { Form } from '@annium/forms-react'
import { ValueField } from '@annium/forms-react-mui'
import { useStore } from '@annium/utils/dist/helpers'
import Button from '@material-ui/core/Button'
import Grid from '@material-ui/core/Grid'
import TextField from '@material-ui/core/TextField'
import { observer } from 'mobx-react-lite'
import { MinimalPage } from 'public/layouts/MinimalPage'
import React, { useCallback } from 'react'
import { Link } from 'shared/components/Link'
import { Loader } from 'shared/components/Loader'
import { validate } from 'shared/utils/forms'

import { Store } from './store'
import { useStyles } from './styles'
import { DataValidator } from './validators'


export const LoginPage = observer(() => {
  const store = useStore(new Store())
  const form = store.form
  const classes = useStyles()

  const isFormSubmittable = !store.state.isLoading && form.hasStatus(Status.Success) &&
    form.login.hasBeenTouched && form.password.hasBeenTouched
  const handleChangeSubmit = useCallback((e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' && isFormSubmittable)
      store.login()
  }, [isFormSubmittable, store])

  return (
    <MinimalPage title="sign in">
      <Loader direction="column" align="stretch" justify="flex-start" isLoading={store.state.isLoading}>
        <Form state={form} onChange={validate(DataValidator)}>
          <ValueField field={form.login}>
            <TextField
              variant="outlined"
              margin="normal"
              required={true}
              fullWidth={true}
              label="Login"
              name="login"
              autoComplete="login"
              autoFocus={true}
              onKeyDown={handleChangeSubmit}
            />
          </ValueField>
          <ValueField field={form.password}>
            <TextField
              variant="outlined"
              margin="normal"
              required={true}
              fullWidth={true}
              name="password"
              label="Password"
              type="password"
              autoComplete="current-password"
              onKeyDown={handleChangeSubmit}
            />
          </ValueField>
          <Button
            fullWidth={true}
            variant="contained"
            color="primary"
            className={classes.submit}
            disabled={!isFormSubmittable}
            onClick={store.login}
          >
            Sign In
          </Button>
          <Grid container={true}>
            <Grid item={true} xs={true}>
              <Link to="/restore-access" variant="body2">Forgot password?</Link>
            </Grid>
            <Grid item={true}>
              No account? <Link to="/register" variant="body2">Sign Up</Link>
            </Grid>
          </Grid>
        </Form>
      </Loader>
    </MinimalPage>
  )
})
