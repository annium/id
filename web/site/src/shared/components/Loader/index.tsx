import CircularProgress from '@material-ui/core/CircularProgress'
import { CSSProperties } from '@material-ui/styles'
import React, { Children, ReactNode } from 'react'

import { useStyles } from './styles'


export type Props = {
  direction: 'row' | 'column'
  align: CSSProperties['alignItems']
  justify: CSSProperties['justifyContent']
  isLoading: boolean
} & { children?: ReactNode }

export const Loader = ({ direction, align, justify, isLoading, children }: Props) => {
  const classes = useStyles()

  if (!isLoading)
    return Children.count(children) ? children as JSX.Element : null

  const style = {
    flexDirection: direction,
    alignItems: align,
    justifyContent: justify,
  }

  return (
    <div className={classes.wrapper}>
      <div className={classes.container} style={style}>
        {children}
      </div>
      <div className={classes.overlay}>
        <CircularProgress />
      </div>
    </div>
  )
}
