import { useInjection } from 'public/config/di'
import { services } from 'public/config/di/services'
import React, { useEffect } from 'react'
import { IRouterStore } from 'shared/stores/RouterStore'


export const HomePage = (): JSX.Element => {
  const router = useInjection<IRouterStore>(services.RouterStore)

  useEffect(() => {
    router.goToHomeOrStartup()
  }, [router])

  return (
    <div />
  )
}
