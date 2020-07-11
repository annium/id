import Typography from '@material-ui/core/Typography'
import React from 'react'

import { useStyles } from './styles'

export type Props = { reason: string }

export const Failure = ({ reason }: Props) => {
  const classes = useStyles()

  return (
    <div className={classes.section}>
      <Typography variant="body1">
        Registration failed.
      </Typography>
      <Typography variant="body1">
        Possible reason: <span className={classes.failure}>{reason}</span>.
      </Typography>
      <Typography variant="body1">
        <p>Please, contact <a href="mailto:support@annium">support</a> to let us help you</p>
      </Typography>
    </div>
  )
}
