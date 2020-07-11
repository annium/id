import { makeStyles } from '@material-ui/core/styles'
import { flexColumn } from 'shared/styles/mixins'


export const useStyles = makeStyles(theme => ({
  page: {
    flex: 1,
    ...flexColumn(),
  },
  container: {
    ...flexColumn('center'),
    backgroundColor: theme.palette.background.paper,
    marginTop: theme.spacing(8),
    padding: theme.spacing(2),
    borderRadius: theme.spacing(2),
  },
  avatar: {
    margin: theme.spacing(1),
  },
  credentials: {
    marginTop: theme.spacing(3),
  },
}))
