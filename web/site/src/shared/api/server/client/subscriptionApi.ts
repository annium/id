// imports
import { HttpClient, HttpResponse } from '@annium/client-http'

import {
    AccountPublicResponse,
} from './shared'

// exports
export type CreateSubscriptionRequest = {
    masterId: string
    followerId: string
}

export type SubscriptionResponse = {
    id: string
    master: AccountPublicResponse
    follower: AccountPublicResponse
    status: SubscriptionStatus
}

export enum SubscriptionStatus {
    Active = 0,
}


// api
export const subscriptionApi = (client: HttpClient) => ({
    createSubscription: (
        body: CreateSubscriptionRequest,
    ): Promise<HttpResponse> => client
        .post(`subscriptions`, {}, body),
    getMasterSubscriptions: (
    ): Promise<HttpResponse<SubscriptionResponse[]>> => client
        .get(`subscriptions/master`, {}),
    getFollowerSubscriptions: (
    ): Promise<HttpResponse<SubscriptionResponse[]>> => client
        .get(`subscriptions/follower`, {}),
    deleteSubscription: (
        id: string,
    ): Promise<HttpResponse> => client
        .delete(`subscriptions/${id}`, {}),
})
