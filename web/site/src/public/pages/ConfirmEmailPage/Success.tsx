import Typography from '@material-ui/core/Typography'
import React from 'react'

import { useStyles } from './styles'


export const Success = () => {
  const classes = useStyles()

  return (
    <div className={classes.section}>
      <Typography variant="body1" className={classes.success}>
        Your email was confirmed.
      </Typography>
      <Typography variant="body1">
        You will need to set your password at profile page.
      </Typography>
    </div>
  )
}
