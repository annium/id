// imports
import { HttpClient, HttpResponse } from '@annium/client-http'



// exports

export type CompanyClaimResponse = {
  id: string
  appId: string
  key: string
  name: string
}

export type CreateCompanyClaimRequest = {
  appId: string
  key: string
  name: string
}

export type UpdateCompanyClaimRequestBody = {
  key: string
  name: string
}


// api
export const companyClaimApi = (client: HttpClient) => ({
  createCompanyClaim: (
    body: CreateCompanyClaimRequest,
  ): Promise<HttpResponse<string>> => client
    .post(`companies/claims`, {}, body),
  listCompanyClaims: (
    appId: string,
  ): Promise<HttpResponse<CompanyClaimResponse[]>> => client
    .get(`companies/claims`, { appId }),
  updateCompanyClaim: (
    claimId: string,
    body: UpdateCompanyClaimRequestBody,
  ): Promise<HttpResponse> => client
    .put(`companies/claims/${claimId}`, {}, body),
  deleteCompanyClaim: (
    claimId: string,
  ): Promise<HttpResponse> => client
    .delete(`companies/claims/${claimId}`, {}),
})
