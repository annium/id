import Tab from '@material-ui/core/Tab'
import React from 'react'
import { IRouterStore } from 'shared/stores/RouterStore'

type LinkTabProps = {
  router: IRouterStore
  label: string
  href: string
}

export const LinkTab = (props: LinkTabProps) => {
  const { router, href } = props

  const handleClick = (event: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
    event.preventDefault()
    router.go(href)
  }

  return (
    <Tab component="a" onClick={handleClick} {...props} value={href} />
  )
}
