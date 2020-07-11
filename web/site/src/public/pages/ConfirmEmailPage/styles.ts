import { makeStyles } from '@material-ui/core/styles'

export const useStyles = makeStyles(theme => ({
  section: {
    margin: theme.spacing(3, 0, 0),
    textAlign: 'center',
  },
  success: {
    color: theme.palette.primary.dark,
  },
  failure: {
    color: theme.palette.error.dark,
  },
}))
