import blue from '@material-ui/core/colors/blue'
import pink from '@material-ui/core/colors/pink'
import red from '@material-ui/core/colors/red'
import { createMuiTheme, makeStyles } from '@material-ui/core/styles'

export const theme = createMuiTheme({
    palette: {
        primary: blue,
        secondary: pink,
        error: red,
    },
})

export const useStyles = makeStyles(th => ({
    '@global': {
        html: {
            display: 'flex',
            flexDirection: 'column',
            minHeight: '100vh',
        },
        body: {
            flex: 1,
            display: 'flex',
            flexDirection: 'column',
            backgroundColor: th.palette.common.white,
        },
    },
    '@global #root': {
        flex: 1,
        display: 'flex',
        flexDirection: 'column',
    },
}))
