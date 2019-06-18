import { I18nProvider } from '@lingui/react'
import CssBaseline from '@material-ui/core/CssBaseline'
import { ThemeProvider } from '@material-ui/styles'
import React from 'react'
import ReactDOM from 'react-dom'

import { context } from './context'
import { i18n } from './i18n'
import { Routes } from './routes'
import { theme } from './styles'


ReactDOM.render(
  (
    <I18nProvider i18n={i18n} language={i18n.language}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <Routes />
      </ThemeProvider>
    </I18nProvider>
  ),
  document.getElementById('root'),
)

Object.defineProperty(window, 's', { get: context.getState })
