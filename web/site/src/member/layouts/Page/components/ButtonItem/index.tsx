import ListItem from '@material-ui/core/ListItem'
import ListItemIcon from '@material-ui/core/ListItemIcon'
import ListItemText from '@material-ui/core/ListItemText'
import React from 'react'

type Props = {
  label: string
  icon: JSX.Element
  onClick(): void
}

export const ButtonItem = ({ onClick, icon, label }: Props) => (
  <ListItem button={true} onClick={onClick} title={label}>
    <ListItemIcon>{icon}</ListItemIcon>
    <ListItemText primary={label} />
  </ListItem>
)
