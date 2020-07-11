import { makeStyles } from '@material-ui/core/styles'


export const useStyles = makeStyles(theme => ({
  logo: {
    backgroundImage: `url(/images/logo/logo_64.png)`,
    backgroundSize: 'contain',
  },
  small: {
    width: theme.spacing(4),
    height: theme.spacing(4),
  },
  medium: {
    width: theme.spacing(6),
    height: theme.spacing(6),
  },
  large: {
    width: theme.spacing(8),
    height: theme.spacing(8),
  },
}))
