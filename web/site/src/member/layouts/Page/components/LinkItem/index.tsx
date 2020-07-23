import React from 'react'
import { Link } from 'shared/components/Link'

import { useStyles } from './styles'

type Props = {
  icon: JSX.Element
  label: string
  to: string
}

export const LinkItem = ({ icon, label, to }: Props) => {
  const classes = useStyles()

  return (
      <Link to={to} underline="none" color="inherit" title={label} className={classes.link}>
        {icon}
      </Link>
  )
}
