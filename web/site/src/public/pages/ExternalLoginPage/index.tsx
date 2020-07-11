import { observer } from 'mobx-react-lite'
import { useInjection } from 'public/config/di'
import { services } from 'public/config/di/services'
import { Routes } from 'public/routes'
import { useEffect } from 'react'
import { TokensResponse } from 'shared/api/id/client/shared'
import { tokenStorage } from 'shared/api/id/tokenStorage'
import { IMeStore } from 'shared/stores/MeStore'
import { IRouterStore } from 'shared/stores/RouterStore'


export const ExternalLoginPage = observer(() => {
  const me = useInjection<IMeStore>(services.MeStore)
  const router = useInjection<IRouterStore>(services.RouterStore)

  useEffect(() => {
    if (!me.hasAccess)
      tokenStorage.set(router.get<TokensResponse>(Routes.externalLogin))

    router.goToHomeOrStartup()
  }, [me, router])

  return null
})
