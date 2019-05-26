import cx from 'classnames'
import React, { Children, ReactNode } from 'react'

import styles from './index.module.scss'


export type Props = {
  isLoading: boolean
  className?: string
} & { children?: ReactNode }

export const Loader = ({ isLoading, className, children }: Props) => {
  const cls = cx(styles.loader, className)
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