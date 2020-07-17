import { makeStyles } from '@material-ui/core/styles'
import { flexRow } from 'shared/styles/mixins'


export const useStyles = makeStyles(theme => ({
  itemActions: {
    ...flexRow('center', 'flex-end'),
    '& *:not(:first-child)': {
      marginLeft: theme.spacing(1),
    },
  },
}))
