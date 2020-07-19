import cx from 'classnames'
import React, { ReactNode } from 'react'

import { useStyles } from './styles'

type Props = {
  className?: string
  children: ReactNode
}

export const CentricGrid = ({ className, children }: Props) => {
  const classes = useStyles()

  return (
    <div className={cx(className, classes.container)}>
      {children}
    </div>
  )
}
