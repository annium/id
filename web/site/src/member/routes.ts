export const routes = {
  accounts: {
    list: '/member/accounts',
    create: '/member/accounts/create',
    update: '/member/accounts/:id',
  },
  dashboard: '/member',
  following: {
    list: '/member/following',
    create: '/member/following/create',
  },
  profile: '/member/profile',
  followers: {
    listAll: '/member/followers',
    listAccount: '/member/followers/:accountId',
    create: '/member/followers/:accountId/create',
  },
}
