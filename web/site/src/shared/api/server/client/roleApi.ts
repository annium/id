// imports
import { HttpClient, HttpResponse } from '@annium/client-http'


// exports
export type AddClaimToRoleRequestBase = {
    value: string
}

export type CreateRoleRequest = {
    appId: string
    key: string
    name: string
}

export type RoleResponse = {
    id: string
    appId: string
    key: string
    name: string
    claims: Object[]
}

export type UpdateRoleRequestBase = {
    key: string
    name: string
}


// api
export const roleApi = (client: HttpClient) => ({
    createRole: (
        body: CreateRoleRequest,
    ): Promise<HttpResponse<string>> => client
        .post(`roles`, {}, body),
    listRoles: (
        appId: string,
    ): Promise<HttpResponse<RoleResponse[]>> => client
        .get(`roles`, { appId }),
    updateRole: (
        roleId: string,
        body: UpdateRoleRequestBase,
    ): Promise<HttpResponse> => client
        .put(`roles/${roleId}`, {}, body),
    addClaimToRole: (
        claimId: string,
        roleId: string,
        body: AddClaimToRoleRequestBase,
    ): Promise<HttpResponse> => client
        .post(`roles/${roleId}/claims/${claimId}`, {}, body),
    deleteClaimFromRole: (
        claimId: string,
        roleId: string,
    ): Promise<HttpResponse> => client
        .delete(`roles/${roleId}/claims/${claimId}`, {}),
    deleteRole: (
        roleId: string,
    ): Promise<HttpResponse> => client
        .delete(`roles/${roleId}`, {}),
})
