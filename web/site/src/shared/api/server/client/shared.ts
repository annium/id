export type AccountPublicResponse = {
    id: string
    user: UserResponse
    name: string
    exchange: Exchange
    isTest: boolean
}
export type AccountResponse = {
    id: string
    name: string
    exchange: Exchange
    isTest: boolean
    key: string
    secret: string
}
export enum Exchange {
    BitMEX = 0,
}
export type UserResponse = {
    id: string
    login: string
}
