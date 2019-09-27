import { makeStyles } from '@material-ui/core/styles'

export const useStyles = makeStyles(theme => ({
    root: {
        alignItems: 'stretch',
        display: 'flex',
        flex: 1,
        flexDirection: 'column',
        justifyContent: 'flex-start',
    },
    navigation: {
        margin: '0 auto',
    },
    footer: {
        padding: theme.spacing(2),
        marginTop: 'auto',
    },
}))
