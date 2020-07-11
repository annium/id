import Typography from '@material-ui/core/Typography'
import { observer } from 'mobx-react-lite'
import React from 'react'

import { useStyles } from './styles'


type Props = { email: string }

export const RestoreSuccess = observer(({ email }: Props) => {
  const classes = useStyles()

  return (
    <div className={classes.section}>
      <Typography variant="body1" className={classes.success}>
        Access restore link successfully sent to <b>{email}</b>.
      </Typography>
      <Typography variant="body1" className={classes.success}>
        You can close this page.
      </Typography>
    </div>
  )
})
