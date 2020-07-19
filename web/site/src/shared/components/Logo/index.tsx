import cx from 'classnames'
import React from 'react'

import { useStyles } from './styles'

type Props = {
  className?: string
  size: 'small' | 'medium' | 'large'
  onClick?(): void;
}

export const Logo = ({ className, size, onClick }: Props) => {
  const classes = useStyles()

  return (
    <div
      className={cx(classes.logo, className, classes[size])}
      onClick={onClick}
    />
  )
}
