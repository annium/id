import { makeStyles } from '@material-ui/core/styles'
import { flexColumn, flexRow } from 'shared/styles/mixins'

export const useStyles = makeStyles(theme => ({
  page: {
    flex: 1,
    ...flexRow('stretch'),
  },
  sidebar: {
    flexShrink: 0,
    whiteSpace: 'nowrap',
  },
  sidebarOpen: {
    width: theme.spacing(30),
    transition: theme.transitions.create('width', {
      easing: theme.transitions.easing.sharp,
      duration: theme.transitions.duration.enteringScreen,
    }),
  },
  sidebarClose: {
    width: theme.spacing(7),
    transition: theme.transitions.create('width', {
      easing: theme.transitions.easing.sharp,
      duration: theme.transitions.duration.leavingScreen,
    }),
    overflowX: 'hidden',
  },
  toggle: {
    ...flexRow('center', 'space-between'),
    ...theme.mixins.toolbar,
    cursor: 'pointer',
  },
  logo: {
    marginLeft: theme.spacing(0.5),
  },
  content: {
    flex: 1,
    ...flexColumn(),
  },
}))
