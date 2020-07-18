import Tabs from '@material-ui/core/Tabs'
import TabContext from '@material-ui/lab/TabContext'
import { useInjection } from 'member/config/di'
import { services } from 'member/config/di/services'
import React, { ReactNode } from 'react'
import { Switch } from 'react-router-dom'
import { IRouterStore } from 'shared/stores/RouterStore'

import { LinkTab } from './components/LinkTab'


type LinkTabsProps = {
  tabs: Record<string, string>
  children: ReactNode
}

export const LinkTabs = ({ tabs, children }: LinkTabsProps) => {
  const router = useInjection<IRouterStore>(services.RouterStore)
  const tab = Math.max(Object.keys(tabs).findIndex(path => router.isAt(path)), 0)

  return (
    <TabContext value={router.location.pathname}>
      <div>
        <Tabs value={tab}>
          {Object.keys(tabs).map(path => (
            <LinkTab key={path} router={router} label={tabs[path]} href={path} />
          ))}
        </Tabs>
      </div>
      <Switch>
        {children}
      </Switch>
    </TabContext>
  )
}
