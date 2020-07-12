import { StrictSchema } from 'morphism'
import { AppResponse } from 'shared/api/server/client/appApi'

export type App = {
  id: string
  name: string
}

export const AppResponseSchema: StrictSchema<App, AppResponse> = {
  id: 'id',
  name: 'name',
}

