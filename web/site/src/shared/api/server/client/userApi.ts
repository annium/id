// imports
import { HttpClient, HttpResponse } from '@annium/client-http'

import {
    UserResponse,
} from './shared'

// exports

// api
export const userApi = (client: HttpClient) => ({
    findUsers: (
        login: string,
    ): Promise<HttpResponse<UserResponse[]>> => client
        .get(`users`, { login }),
})
