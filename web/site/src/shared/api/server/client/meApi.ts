// imports
import { HttpClient, HttpResponse } from '@annium/client-http'

import {
    AccountResponse,
} from './shared'

// exports
export type MeResponse = {
    id: string
    email: string
    login: string
    accounts: AccountResponse[]
}

export type SyncMeRequest = {
    email: string
    login: string
}


// api
export const meApi = (client: HttpClient) => ({
    syncMe: (
        body: SyncMeRequest,
    ): Promise<HttpResponse> => client
        .post(`me`, {}, body),
    getMe: (
    ): Promise<HttpResponse<MeResponse>> => client
        .get(`me`, {}),
})
