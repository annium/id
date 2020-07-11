import Grid from '@material-ui/core/Grid'
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
      <Grid container={true} spacing={3}>
        <Grid item={true} sm={1} md={1} />
        <Grid item={true} xs={12} sm={10} md={10}>
          {children}
        </Grid>
      </Grid>
    </div>
  )
}
