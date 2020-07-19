import { makeStyles } from '@material-ui/core/styles'
import { flexColumn, flexRow } from 'shared/styles/mixins'

export const useStyles = makeStyles(theme => ({
  page: {
    flex: 1,
    ...flexRow('stretch'),
  },
  appBar: {
    backgroundColor: theme.palette.grey.A400,
  },
  logo: {
    marginLeft: theme.spacing(0.5),
  },
  content: {
    flex: 1,
    ...flexColumn(),
    marginTop: theme.spacing(8),
  },
}))
