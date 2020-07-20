// imports
import { HttpClient, HttpResponse } from '@annium/client-http'

import {
  UserResponse,
} from './shared'

// exports

export type CompanyResponse = {
  id: string
  name: string
}

export type RegisterCompanyRequest = {
  parentId: string | null
  name: string
}

export type UpdateCompanyRequestBody = {
  parentId: string | null
  name: string
}


// api
export const companyApi = (client: HttpClient) => ({
  registerCompany: (
    body: RegisterCompanyRequest,
  ): Promise<HttpResponse<string>> => client
    .post(`companies`, {}, body),
  findCompanies: (
    query: string,
  ): Promise<HttpResponse<CompanyResponse[]>> => client
    .get(`companies`, { query }),
  listMyCompanies: (
  ): Promise<HttpResponse<CompanyResponse[]>> => client
    .get(`companies/my`, {}),
  getCompany: (
    companyId: string,
  ): Promise<HttpResponse<CompanyResponse>> => client
    .get(`companies/${companyId}`, {}),
  getCompanyUsers: (
    companyId: string,
  ): Promise<HttpResponse<UserResponse[]>> => client
    .get(`companies/${companyId}/users`, {}),
  updateCompany: (
    companyId: string,
    body: UpdateCompanyRequestBody,
  ): Promise<HttpResponse> => client
    .put(`companies/${companyId}`, {}, body),
  setCompanyOwner: (
    companyId: string,
    userId: string,
  ): Promise<HttpResponse> => client
    .put(`companies/${companyId}/owner/${userId}`, {}),
  unregisterCompany: (
    companyId: string,
  ): Promise<HttpResponse> => client
    .delete(`companies/${companyId}`, {}),
})
