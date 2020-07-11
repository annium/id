import { Status } from '@annium/forms'
import { Form } from '@annium/forms-react'
import { ValueField } from '@annium/forms-react-mui'
import Button from '@material-ui/core/Button'
import Grid from '@material-ui/core/Grid'
import TextField from '@material-ui/core/TextField'
import { observer } from 'mobx-react-lite'
import React, { useCallback } from 'react'
import { Link } from 'shared/components/Link'
import { Loader } from 'shared/components/Loader'
import { validate } from 'shared/utils/forms'

import { Store } from './store'
import { useStyles } from './styles'
import { DataValidator } from './validators'


type Props = { store: Store }

export const RegistrationForm = observer(({ store }: Props) => {
  const form = store.form

  const classes = useStyles()

  const isFormSubmittable = !store.state.isLoading && form.hasStatus(Status.Success) &&
    form.login.hasBeenTouched && form.email.hasBeenTouched
  const handleChangeSubmit = useCallback((e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' && isFormSubmittable)
      store.register()
  }, [isFormSubmittable, store])

  return (
    <Loader direction="column" align="stretch" justify="flex-start" isLoading={store.state.isLoading}>
      <Form state={form} onChange={validate(DataValidator)}>
        <ValueField field={form.email}>
          <TextField
            variant="outlined"
            margin="normal"
            required={true}
            fullWidth={true}
            label="Email"
            name="email"
            autoComplete="email"
            autoFocus={true}
            onKeyDown={handleChangeSubmit}
          />
        </ValueField>
        <ValueField field={form.login}>
          <TextField
            variant="outlined"
            margin="normal"
            required={true}
            fullWidth={true}
            name="login"
            label="login"
            type="login"
            autoComplete="login"
            onKeyDown={handleChangeSubmit}
          />
        </ValueField>
        <Button
          fullWidth={true}
          variant="contained"
          color="primary"
          className={classes.submit}
          disabled={!isFormSubmittable}
          onClick={store.register}
        >
          Register
        </Button>
        <Grid container={true}>
          <Grid item={true} xs={true}>
            <Link to="/restore-access" variant="body2">Forgot password?</Link>
          </Grid>
          <Grid item={true}>
            Have account? <Link to="/login" variant="body2">Log in</Link>
          </Grid>
        </Grid>
      </Form>
    </Loader>
  )
})
