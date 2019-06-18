import { Field, Form, pattern, required, useForm } from '@annium/forms'
import Avatar from '@material-ui/core/Avatar'
import Box from '@material-ui/core/Box'
import Button from '@material-ui/core/Button'
import Checkbox from '@material-ui/core/Checkbox'
import Container from '@material-ui/core/Container'
import FormControlLabel from '@material-ui/core/FormControlLabel'
import Grid from '@material-ui/core/Grid'
import Link from '@material-ui/core/Link'
import TextField from '@material-ui/core/TextField'
import Typography from '@material-ui/core/Typography'
import LockOutlinedIcon from '@material-ui/icons/LockOutlined'
import React from 'react'

import { connect, Store } from '../../store'

import { useStyles } from './styles'


type LoginData = { email: string; password: string; remember: boolean }

const log = console.log.bind(console, 'LoginPage')

type OwnProps = {}
type SelectorProps = Pick<Store['startup'], 'location'>

export const LoginPage = connect<OwnProps, SelectorProps>(
  ({ startup }) => ({ location: startup.location }),
  ({ location }: OwnProps & SelectorProps) => {
    const form = useForm<LoginData>({ email: '', password: '', remember: false })
    const classes = useStyles()

    log('render', location)
    const isDataValid = form.isValid &&
      !form.untouchedFields.includes('email') &&
      !form.untouchedFields.includes('password')

    return (
      <Container component="main" maxWidth="xs">
        <div className={classes.paper}>
          <Avatar className={classes.avatar}>
            <LockOutlinedIcon />
          </Avatar>
          <Typography component="h1" variant="h5">
            Annium ID Sign in
          </Typography>
          <Form state={form}>
            <Field
              name="email"
              validators={[
                required({ message: 'Specify email' }),
                pattern({ pattern: /.+@.+/, message: 'Specify valid email' }),
              ]}
              hasMessage={true}
            >
              <TextField
                variant="outlined"
                margin="normal"
                required={true}
                fullWidth={true}
                id="email"
                label="Email Address"
                name="email"
                autoComplete="email"
                autoFocus={true}
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
                id="password"
                autoComplete="current-password"
              />
            </Field>
            <FormControlLabel
              control={
                <Field name="remember">
                  <Checkbox value="remember" color="primary" />
                </Field>
              }
              label="Remember me"
            />
            <Button
              fullWidth={true}
              variant="contained"
              color="primary"
              className={classes.submit}
              disabled={!isDataValid}
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
