import { setupI18n } from '@lingui/core'

const catalogs = {
  en: require('./locales/en/messages'),
  ru: require('./locales/ru/messages'),
}

// TODO: use other way
const params = new URLSearchParams(window.location.search)
const language = params.get('lang') || 'en'

const missing = (lang: string, id: string) => {
  const message = `No translation for '${id}' in '${lang}'`
  alert(message)

  return message
}

export const i18n = setupI18n({ catalogs, language, missing })
