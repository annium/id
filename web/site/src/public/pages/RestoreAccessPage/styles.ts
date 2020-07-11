import { makeStyles } from '@material-ui/core/styles'

export const useStyles = makeStyles(theme => ({
  submit: {
    margin: theme.spacing(3, 0, 2),
  },
  section: {
    margin: theme.spacing(3, 0, 0),
    textAlign: 'center',
  },
  success: {
    color: theme.palette.primary.dark,
    textAlign: 'center',
  },
}))
