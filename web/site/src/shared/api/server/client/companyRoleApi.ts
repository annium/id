// imports
import { HttpClient, HttpResponse } from '@annium/client-http'


// exports
export type AddCompanyClaimToCompanyRoleRequestBody = {
  value: string
}

export type CompanyRoleResponse = {
  id: string
  appId: string
  key: string
  name: string
  claims: Object[]
}

export type CreateCompanyRoleRequest = {
  appId: string
  key: string
  name: string
}

export type UpdateCompanyRoleRequestBody = {
  key: string
  name: string
}


// api
export const companyRoleApi = (client: HttpClient) => ({
  createRole: (
    body: CreateCompanyRoleRequest,
  ): Promise<HttpResponse<string>> => client
    .post(`companies/roles`, {}, body),
  listRoles: (
    appId: string,
  ): Promise<HttpResponse<CompanyRoleResponse[]>> => client
    .get(`companies/roles`, { appId }),
  updateRole: (
    roleId: string,
    body: UpdateCompanyRoleRequestBody,
  ): Promise<HttpResponse> => client
    .put(`companies/roles/${roleId}`, {}, body),
  addClaimToRole: (
    claimId: string,
    roleId: string,
    body: AddCompanyClaimToCompanyRoleRequestBody,
  ): Promise<HttpResponse> => client
    .post(`companies/roles/${roleId}/claims/${claimId}`, {}, body),
  deleteClaimFromRole: (
    claimId: string,
    roleId: string,
  ): Promise<HttpResponse> => client
    .delete(`companies/roles/${roleId}/claims/${claimId}`, {}),
  deleteRole: (
    roleId: string,
  ): Promise<HttpResponse> => client
    .delete(`companies/roles/${roleId}`, {}),
})
