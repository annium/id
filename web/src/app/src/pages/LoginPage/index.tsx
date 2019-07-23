import { Field, Form, required, useForm } from '@annium/forms'
import Avatar from '@material-ui/core/Avatar'
import Box from '@material-ui/core/Box'
import Button from '@material-ui/core/Button'
import Container from '@material-ui/core/Container'
import Grid from '@material-ui/core/Grid'
import Link from '@material-ui/core/Link'
import TextField from '@material-ui/core/TextField'
import Typography from '@material-ui/core/Typography'
import LockOutlinedIcon from '@material-ui/icons/LockOutlined'
import { Location, LocationDescriptorObject } from 'history'
import React, { useEffect } from 'react'
import { RouteComponentProps } from 'react-router-dom'

import { authActions } from '../../data/auth'
import { connect, Store } from '../../store'

import { useStyles } from './styles'

type LoginData = { login: string; password: string }

const log = console.log.bind(console, 'LoginPage')

type OwnProps = RouteComponentProps
type SelectorProps = {
  auth: Store['auth']
  location: Store['startup']['location']
}

export const LoginPage = connect<OwnProps, SelectorProps>(
  ({ auth, startup }) => ({ auth, location: startup.location }),
  ({ auth, location, history }: OwnProps & SelectorProps) => {
    useEffect(
      () => {
        log('update', 'ensure access')
        ensureAccess(auth, location, history)
      },
      [auth, location, history],
    )

    const form = useForm<LoginData>({ login: '', password: '' })
    const classes = useStyles()

    const isDataValid = form.isValid &&
      !form.untouchedFields.includes('login') &&
      !form.untouchedFields.includes('password')

    return (
      <Container className={classes.page} component="main" maxWidth="xs">
        <div className={classes.container}>
          <Avatar className={classes.avatar}>
            <LockOutlinedIcon />
          </Avatar>
          <Typography component="h1" variant="h5">
            Annium ID Sign in
          </Typography>
          <Form state={form}>
            <Field
              name="login"
              validators={[
                required({ message: 'Specify login' }),
              ]}
              hasMessage={true}
            >
              <TextField
                variant="outlined"
                margin="normal"
                required={true}
                fullWidth={true}
                label="Login"
                name="login"
                autoComplete="login"
                autoFocus={true}
                onKeyDown={handleChangeSubmit(form.data)}
              />
            </Field>
            <Field
              name="password"
              validators={[
                required({ message: 'Specify password' }),
              ]}
              hasMessage={true}
            >
              <TextField
                variant="outlined"
                margin="normal"
                required={true}
                fullWidth={true}
                name="password"
                label="Password"
                type="password"
                autoComplete="current-password"
                onKeyDown={handleChangeSubmit(form.data)}
              />
            </Field>
            <Button
              fullWidth={true}
              variant="contained"
              color="primary"
              className={classes.submit}
              disabled={!isDataValid}
              onClick={handleLogin(form.data)}
            >
              Sign In
            </Button>
            <Grid container={true}>
              <Grid item={true} xs={true}>
                <Link href="#" variant="body2">
                  Forgot password?
              </Link>
              </Grid>
              <Grid item={true}>
                <Link href="#" variant="body2">
                  Don't have an account? Sign Up
                </Link>
              </Grid>
            </Grid>
          </Form>
        </div>
        <Box mt={5}>
          <Typography variant="body2" color="textSecondary" align="center">
            Built with love by the <Link color="inherit" href="https://annium.com/">Annium</Link> team.
          </Typography>
        </Box>
      </Container >
    )
  },
)

const handleChangeSubmit = (loginData: LoginData) => (e: React.KeyboardEvent<HTMLInputElement>) => {
  if (e.key === 'Enter')
    handleLogin(loginData)()
}

const handleLogin = ({ login, password }: LoginData) => () =>
  authActions
    .login({ login, password })
    .catch(error => alert(`login failed: ${error}`))

const ensureAccess = (
  auth: SelectorProps['auth'],
  location: Location,
  history: OwnProps['history'],
) => {
  log('checkAccess', auth.hasAccess)
  // if has no access - nothing to do
  if (!auth.hasAccess)
    return

  // go to personal area, if:
  // - user load is finished
  if (auth.user.isSuccess || auth.user.isFailure) {
    const target: LocationDescriptorObject = location.pathname.startsWith('/login') ? { pathname: '/' } : location
    log('checkAccess', 'go to', target)
    history.replace(target)
  }
}
