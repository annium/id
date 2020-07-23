import React from 'react'

import { useStyles } from './styles'

type Props = {
  icon: JSX.Element
  onClick(): void
}

export const ButtonItem = ({ icon, onClick }: Props) => {
  const classes = useStyles()

  return (
    <div className={classes.button} onClick={onClick}>
      {icon}
    </div>
  )
}
