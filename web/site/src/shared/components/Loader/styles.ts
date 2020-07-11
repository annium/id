import { makeStyles } from '@material-ui/core/styles'
import { flexColumn } from 'shared/styles/mixins'


export const useStyles = makeStyles(_ => ({
  wrapper: {
    display: 'flex',
    position: 'relative',
    flex: 1,
    pointerEvents: 'none',
  },
  container: {
    flex: 1,
    pointerEvents: 'none',
    opacity: 0.5,
  },
  overlay: {
    position: 'absolute',
    left: 0,
    right: 0,
    top: 0,
    bottom: 0,
    ...flexColumn('center', 'center'),
    flex: 1,
  },
}))
