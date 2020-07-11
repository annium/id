import cx from 'classnames'
import React from 'react'

import { useStyles } from './styles'

type Props = {
  className?: string
  size: 'small' | 'medium' | 'large'
}

export const Logo = ({ className, size }: Props) => {
  const classes = useStyles()

  return (
    <div className={cx(classes.logo, className, classes[size])} />
  )
}
