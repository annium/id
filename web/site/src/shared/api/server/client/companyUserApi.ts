// imports
import { HttpClient, HttpResponse } from '@annium/client-http'


// exports
export type AddCompanyClaimToCompanyUserRequestBody = {
  value: string
}


// api
export const companyUserApi = (client: HttpClient) => ({
  addUserToCompany: (
    companyId: string,
    userId: string,
  ): Promise<HttpResponse> => client
    .post(`companies/${companyId}/users/${userId}`, {}),
  addCompanyRoleToCompanyUser: (
    companyId: string,
    roleId: string,
    userId: string,
  ): Promise<HttpResponse> => client
    .post(`companies/${companyId}/users/${userId}/roles/${roleId}`, {}),
  deleteCompanyRoleFromCompanyUser: (
    companyId: string,
    roleId: string,
    userId: string,
  ): Promise<HttpResponse> => client
    .delete(`companies/${companyId}/users/${userId}/roles/${roleId}`, {}),
  addCompanyClaimToCompanyUser: (
    claimId: string,
    companyId: string,
    userId: string,
    body: AddCompanyClaimToCompanyUserRequestBody,
  ): Promise<HttpResponse> => client
    .post(`companies/${companyId}/users/${userId}/claims/${claimId}`, {}, body),
  deleteCompanyClaimFromCompanyUser: (
    claimId: string,
    companyId: string,
    userId: string,
  ): Promise<HttpResponse> => client
    .delete(`companies/${companyId}/users/${userId}/claims/${claimId}`, {}),
  deleteUserFromCompany: (
    companyId: string,
    userId: string,
  ): Promise<HttpResponse> => client
    .delete(`companies/${companyId}/users/${userId}`, {}),
})
