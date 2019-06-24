import { makeStyles } from '@material-ui/core/styles'

export const useStyles = makeStyles(theme => ({
    root: {
        alignItems: 'stretch',
        display: 'flex',
        flex: 1,
        flexDirection: 'column',
        justifyContent: 'flex-start',
    },
    footer: {
        padding: theme.spacing(2),
        marginTop: 'auto',
    },
}))
