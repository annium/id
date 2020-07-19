import { StrictSchema } from 'morphism'
import { CompanyResponse } from 'shared/api/server/client/companyApi'

export type Company = {
  id: string
  name: string
}

export const CompanyResponseSchema: StrictSchema<Company, CompanyResponse> = {
  id: 'id',
  name: 'name',
}

