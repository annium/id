import MaterialLink, { LinkProps as MaterialLinkProps } from '@material-ui/core/Link'
import React from 'react'
import { Link as RouterLink, LinkProps as RouterLinkProps } from 'react-router-dom'


type Props = RouterLinkProps

export const Link = (props: MaterialLinkProps & RouterLinkProps) => {
  const { children, ...rest } = props
  const component = React.forwardRef<HTMLAnchorElement, Props>((innerProps, ref) => (
    <RouterLink innerRef={ref} {...innerProps} />
  ))

  return (
    <MaterialLink component={component} {...rest}>
      {children}
    </MaterialLink>
  )
}
