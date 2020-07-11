// imports
import { HttpClient, HttpResponse } from '@annium/client-http'


// exports
export type AddClaimToUserRequestBody = {
  value: string
}


// api
export const userApi = (client: HttpClient) => ({
  addRoleToUser: (
    roleId: string,
    userId: string,
  ): Promise<HttpResponse> => client
    .post(`users/${userId}/roles/${roleId}`, {}),
  deleteRoleFromUser: (
    roleId: string,
    userId: string,
  ): Promise<HttpResponse> => client
    .delete(`users/${userId}/roles/${roleId}`, {}),
  addClaimToUser: (
    claimId: string,
    userId: string,
    body: AddClaimToUserRequestBody,
  ): Promise<HttpResponse> => client
    .post(`users/${userId}/claims/${claimId}`, {}, body),
  deleteClaimFromUser: (
    claimId: string,
    userId: string,
  ): Promise<HttpResponse> => client
    .delete(`users/${userId}/claims/${claimId}`, {}),
})
