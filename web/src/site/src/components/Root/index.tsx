import BottomNavigation from '@material-ui/core/BottomNavigation'
import BottomNavigationAction from '@material-ui/core/BottomNavigationAction'
import Icon from '@material-ui/core/Icon'
import FavoriteIcon from '@material-ui/icons/Favorite'
import LocationOnIcon from '@material-ui/icons/LocationOn'
import RestoreIcon from '@material-ui/icons/Restore'
import React, { ReactNode } from 'react'

import { useStyles } from './styles'


export const Root = ({ children }: { children?: ReactNode }) => {
  const classes = useStyles()

  const [value, setValue] = React.useState('recents')

  const handleChange = (e: React.ChangeEvent<{}>, newValue: string) => {
    setValue(newValue)
  }

  return (
    <main className={classes.root}>
      {children}
      <BottomNavigation
        value={value}
        onChange={handleChange}
        className={classes.navigation}
        showLabels={true}
      >
        <BottomNavigationAction label="Recents" value="recents" icon={<RestoreIcon />} />
        <BottomNavigationAction label="Favorites" value="favorites" icon={<FavoriteIcon />} />
        <BottomNavigationAction label="Nearby" value="nearby" icon={<LocationOnIcon />} />
        <BottomNavigationAction label="Folder" value="folder" icon={<Icon>folder</Icon>} />
      </BottomNavigation>
    </main>
  )
}
