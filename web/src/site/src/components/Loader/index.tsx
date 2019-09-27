import cx from 'classnames'
import React, { Children, ReactNode } from 'react'

import { useStyles } from './styles'


export type Props = {
  isLoading: boolean
  className?: string
} & { children?: ReactNode }

export const Loader = ({ isLoading, className, children }: Props) => {
  const classes = useStyles()
  const cls = cx(classes.loader, className)
  const childrenResult = Children.count(children) ? children : <span>LOADING</span>

  if (!isLoading)
    return (
      <div className={cls}>
        {childrenResult}
      </div>
    )

  return (
    <div className={cls}>

      {childrenResult}
    </div>
  )
}
