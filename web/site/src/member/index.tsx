import { NotificationsProvider } from '@annium/utils/dist/components'
import { I18nProvider } from '@lingui/react'
import CssBaseline from '@material-ui/core/CssBaseline'
import { ThemeProvider } from '@material-ui/styles'
import React from 'react'

import { containerContext, ContainerProvider } from './config/di'

// tslint:disable-next-line: ordered-imports
import { i18n } from 'shared/i18n'
import { theme, useStyles } from 'shared/styles'

import { services } from './config/di/services'
import { Module } from './Module'

export const App = () => {
  useStyles()

  return (
    <ContainerProvider>
      <I18nProvider i18n={i18n} language={i18n.language}>
        <ThemeProvider theme={theme}>
          <NotificationsProvider
            context={containerContext}
            storeId={services.NotificationStore}
          >
            <CssBaseline />
            <Module />
          </NotificationsProvider>
        </ThemeProvider>
      </I18nProvider>
    </ContainerProvider>
  )
}
