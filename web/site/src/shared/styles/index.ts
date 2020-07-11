import blue from '@material-ui/core/colors/blue'
import pink from '@material-ui/core/colors/pink'
import red from '@material-ui/core/colors/red'
import { createMuiTheme, makeStyles } from '@material-ui/core/styles'

import { flexColumn } from './mixins'


export const theme = createMuiTheme({
  palette: {
    primary: blue,
    secondary: pink,
    error: red,
  },
  typography: {
    fontSize: 14,
  },
})

export const useStyles = makeStyles(_ => ({
  '@global': {
    html: {
      ...flexColumn(),
      width: '100%',
      minHeight: '100vh',
      backgroundColor: '#f5f5f5',
    },
    body: {
      ...flexColumn(),
      width: '100%',
      minHeight: '100vh',
      backgroundColor: '#f5f5f5',
    },
    '#root': {
      ...flexColumn(),
      width: '100%',
      minHeight: '100vh',
    },
  },
}))
