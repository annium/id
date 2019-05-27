import React, { ReactNode } from 'react'

import { useStyles } from './styles'


export const Root = ({ children }: { children?: ReactNode }) => {
  const classes = useStyles()

  return (
    <div className={classes.root}>
      {children}
    </div>
  )
}
