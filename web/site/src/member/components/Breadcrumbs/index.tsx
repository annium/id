import MaterialBreadcrumbs from '@material-ui/core/Breadcrumbs'
import Typography from '@material-ui/core/Typography'
import React from 'react'
import { Link } from 'shared/components/Link'

import { useStyles } from './styles'

export type BreadcrumbItems = Record<string, string | null>
type Props = {
  items: BreadcrumbItems
}

export const Breadcrumbs = ({ items }: Props) => {
  const classes = useStyles()
  const total = Object.keys(items).length

  return (
    <MaterialBreadcrumbs className={classes.breadcrumbs} aria-label="breadcrumb">
      {Object.keys(items).map((label, i) => {
        const to = items[label]
        const item = <Item key={`${i}-item`} label={label} isLast={i === total - 1} />

        return to ? <Link key={i} color="inherit" to={to}>{item}</Link> : item
      })}
    </MaterialBreadcrumbs>
  )
}

type ItemProps = { label: string, isLast: boolean }

const Item = ({ label, isLast }: ItemProps) => {
  const color = isLast ? 'textPrimary' : 'inherit'

  return (
    <Typography
      key={`label:${label}`}
      component="span"
      variant="h6"
      color={color}
    >
      {label}
    </Typography>
  )
}
