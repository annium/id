import { makeStyles } from '@material-ui/core/styles'
import { flexRow } from 'shared/styles/mixins'


export const useStyles = makeStyles(theme => ({
  paper: {
    padding: theme.spacing(2),
  },
  buttons: {
    ...flexRow('center', 'flex-end'),
    '& *:not(:first-child)': {
      marginLeft: theme.spacing(2),
    },
  },
}))
