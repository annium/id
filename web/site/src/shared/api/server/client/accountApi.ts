// imports
import { HttpClient, HttpResponse } from '@annium/client-http'

import {
    AccountPublicResponse,
    AccountResponse,
    Exchange,
} from './shared'

// exports
export type CreateAccountRequest = {
    exchange: Exchange
    name: string
    isTest: boolean
    key: string
    secret: string
}

export type UpdateAccountRequestBase = {
    exchange: Exchange
    name: string
    isTest: boolean
    key: string
    secret: string
}


// api
export const accountApi = (client: HttpClient) => ({
    createAccount: (
        body: CreateAccountRequest,
    ): Promise<HttpResponse> => client
        .post(`accounts`, {}, body),
    listMyAccounts: (
    ): Promise<HttpResponse<AccountResponse[]>> => client
        .get(`accounts/my`, {}),
    getMyAccount: (
        id: string,
    ): Promise<HttpResponse<AccountResponse>> => client
        .get(`accounts/my/${id}`, {}),
    listAccounts: (
        userId: string,
    ): Promise<HttpResponse<AccountPublicResponse[]>> => client
        .get(`accounts`, { userId }),
    updateAccount: (
        id: string,
        body: UpdateAccountRequestBase,
    ): Promise<HttpResponse> => client
        .put(`accounts/${id}`, {}, body),
    deleteAccount: (
        id: string,
    ): Promise<HttpResponse> => client
        .delete(`accounts/${id}`, {}),
})
