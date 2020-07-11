import ListItem from '@material-ui/core/ListItem'
import ListItemIcon from '@material-ui/core/ListItemIcon'
import ListItemText from '@material-ui/core/ListItemText'
import React from 'react'
import { Link } from 'shared/components/Link'

type Props = {
  to: string
  icon: JSX.Element
  label: string
}

export const LinkItem = ({ to, icon, label }: Props) => (
  <Link to={to} underline="none" color="inherit" title={label}>
    <ListItem button={true}>
      <ListItemIcon>{icon}</ListItemIcon>
      <ListItemText primary={label} />
    </ListItem>
  </Link>
)
