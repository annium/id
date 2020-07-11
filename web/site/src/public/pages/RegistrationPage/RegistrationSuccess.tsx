import Typography from '@material-ui/core/Typography'
import cx from 'classnames'
import { observer } from 'mobx-react-lite'
import React from 'react'

import { useStyles } from './styles'


export const RegistrationSuccess = observer(() => {
  const classes = useStyles()

  return (
    <Typography variant="body1" className={cx(classes.section, classes.success)}>
      To complete registration, confirm your email, following link, we sent to you
    </Typography>
  )
})
