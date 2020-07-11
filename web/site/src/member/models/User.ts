import { StrictSchema } from 'morphism'
import { UserResponse } from 'shared/api/server/client/shared'

export type User = {
  id: string
  login: string
}

export const UserResponseSchema: StrictSchema<User, UserResponse> = {
  id: 'id',
  login: 'login',
}
