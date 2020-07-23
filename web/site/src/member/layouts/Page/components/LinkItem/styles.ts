import { makeStyles } from '@material-ui/core/styles'
import { flexRow } from 'shared/styles/mixins'

export const useStyles = makeStyles(theme => ({
  link: {
    width: theme.spacing(6),
    height: theme.spacing(6),
    ...flexRow('center', 'center'),
    cursor: 'pointer',
    '&:hover': {
      background: '#ffffff30',
    },
  },
}))
