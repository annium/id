import { observer } from 'mobx-react-lite'
import { useInjection } from 'public/config/di'
import { services } from 'public/config/di/services'
import { getLog } from 'public/utils/log'
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
      if (me.hasAccess) {
        log('has access - go to home or startup')
        router.goToHomeOrStartup()
      } else
        log('no access')
    })
  }, [me, router])

  // if not loaded at least once - show loader
  if (!me.hasLoadedOnce) {
    log('not loaded once - show loader')

    return <Loader direction="column" align="center" justify="center" isLoading={me.state.isLoading} />
  }

  return children as JSX.Element
})
