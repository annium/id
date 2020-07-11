import Typography from '@material-ui/core/Typography'
import React from 'react'

import { useStyles } from './styles'


export const InvalidLink = () => {
  const classes = useStyles()

  return (
    <div className={classes.section}>
      <Typography variant="body1" className={classes.failure}>
        Link to this page doesn't seem to be correct.
      </Typography>
      <Typography variant="body1">
        Please, contact <a href="mailto:support@annium">support</a> to let us help you
      </Typography>
    </div>
  )
}
