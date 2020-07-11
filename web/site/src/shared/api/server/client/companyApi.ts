// imports
import { HttpClient, HttpResponse } from '@annium/client-http'


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

export type UserResponse = {
  id: string
  login: string
}


// api
export const companyApi = (client: HttpClient) => ({
  registerCompany: (
    body: RegisterCompanyRequest,
  ): Promise<HttpResponse<string>> => client
    .post(`companies`, {}, body),
  getCompanyInfo: (
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
