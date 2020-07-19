import Tooltip from '@material-ui/core/Tooltip'
import React from 'react'

import { useStyles } from './styles'

type Props = {
  label: string
  icon: JSX.Element
  onClick(): void
}

export const ButtonItem = ({ label, icon, onClick }: Props) => {
  const classes = useStyles()

  return (
    <Tooltip title={label} aria-label={label}>
      <div className={classes.button} onClick={onClick}>
        {icon}
      </div>
    </Tooltip>
  )
}
