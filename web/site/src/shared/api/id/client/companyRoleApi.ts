// imports
import { HttpClient, HttpResponse } from '@annium/client-http'


// exports
export type AddCompanyClaimToCompanyRoleRequestBase = {
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

export type UpdateCompanyRoleRequestBase = {
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
        body: UpdateCompanyRoleRequestBase,
    ): Promise<HttpResponse> => client
        .put(`companies/roles/${roleId}`, {}, body),
    addClaimToRole: (
        claimId: string,
        roleId: string,
        body: AddCompanyClaimToCompanyRoleRequestBase,
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
