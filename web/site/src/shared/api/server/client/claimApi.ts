// imports
import { HttpClient, HttpResponse } from '@annium/client-http'


// exports
export type ClaimResponse = {
  id: string
  appId: string
  key: string
  name: string
}

export type CreateClaimRequest = {
  appId: string
  key: string
  name: string
}

export type UpdateClaimRequestBody = {
  key: string
  name: string
}


// api
export const claimApi = (client: HttpClient) => ({
  createClaim: (
    body: CreateClaimRequest,
  ): Promise<HttpResponse<string>> => client
    .post(`claims`, {}, body),
  listClaims: (
    appId: string,
  ): Promise<HttpResponse<ClaimResponse[]>> => client
    .get(`claims`, { appId }),
  updateClaim: (
    claimId: string,
    body: UpdateClaimRequestBody,
  ): Promise<HttpResponse> => client
    .put(`claims/${claimId}`, {}, body),
  deleteClaim: (
    claimId: string,
  ): Promise<HttpResponse> => client
    .delete(`claims/${claimId}`, {}),
})
