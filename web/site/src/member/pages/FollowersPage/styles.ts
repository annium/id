import { makeStyles } from '@material-ui/core/styles'
import { flexColumn } from 'shared/styles/mixins'


export const useStyles = makeStyles(_ => ({
  container: {
    ...flexColumn,
  },
}))
