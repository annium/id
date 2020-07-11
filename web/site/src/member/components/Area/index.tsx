import { useInjection } from 'member/config/di'
import { services } from 'member/config/di/services'
import { getLog } from 'member/utils/log'
import { observer } from 'mobx-react-lite'
import React, { ReactNode, useEffect } from 'react'
import { Loader } from 'shared/components/Loader'
import { IMeStore } from 'shared/stores/MeStore'
import { IRouterStore } from 'shared/stores/RouterStore'


const log = getLog('components.Area')
type Props = { children?: ReactNode }

export const Area = observer(({ children }: Props) => {
  const me = useInjection<IMeStore>(services.MeStore)
  const router = useInjection<IRouterStore>(services.RouterStore)

  // on mount - check access and redirect
  useEffect(() => {
    log('load me')
    me.load().then(() => {
      log('check access')
      if (me.hasAccess)
        log('has access')
      else {
        log('no access - go to login')
        router.goToLogin()
      }
    })
  }, [me, router])

  const loader = <Loader direction="column" align="center" justify="center" isLoading={me.state.isLoading} />

  // if not loaded at least once - show loader
  if (!me.hasLoadedOnce) {
    log('not loaded once - show loader')

    return loader
  }

  // show loader if no access
  // this one is for moment between user became loaded and router started redirect
  if (!me.hasAccess) {
    log('no access - show loader')

    return loader
  }

  return children as JSX.Element
})
