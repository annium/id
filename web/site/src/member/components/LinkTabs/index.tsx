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
  const options = Object.values(tabs)
    .map((path, index) => ({ path, index }))
    .filter(x => router.isAt(x.path))
    .sort((a, b) => a.path.length > b.path.length ? -1 : 1)
  const tab = options.length ? options[0].index : 0

  return (
    <TabContext value={router.location.pathname}>
      <div>
        <Tabs value={tab}>
          {Object.keys(tabs).map(name => (
            <LinkTab key={tabs[name]} router={router} label={name} href={tabs[name]} />
          ))}
        </Tabs>
      </div>
      <Switch>
        {children}
      </Switch>
    </TabContext>
  )
}
