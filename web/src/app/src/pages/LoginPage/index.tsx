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


const log = console.log.bind(console, 'LoginPage')

type OwnProps = {}
type SelectorProps = Pick<Store['startup'], 'location'>

export const LoginPage = connect<OwnProps, SelectorProps>(
  ({ startup }) => ({ location: startup.location }),
  ({ location }: OwnProps & SelectorProps) => {
    const classes = useStyles()
    log('render', location)

    return (
      <Container component="main" maxWidth="xs">
        <div className={classes.paper}>
          <Avatar className={classes.avatar}>
            <LockOutlinedIcon />
          </Avatar>
          <Typography component="h1" variant="h5">
            Annium ID Sign in
        </Typography>
          <form className={classes.form} noValidate={true}>
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
            <FormControlLabel
              control={<Checkbox value="remember" color="primary" />}
              label="Remember me"
            />
            <Button
              type="submit"
              fullWidth={true}
              variant="contained"
              color="primary"
              className={classes.submit}
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
          </form>
        </div>
        <Box mt={5}>
          <Typography variant="body2" color="textSecondary" align="center">
            Built with love by the <Link color="inherit" href="https://annium.com/">Annium</Link> team.
          </Typography>
        </Box>
      </Container>
    )
  },
)
