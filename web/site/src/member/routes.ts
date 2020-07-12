export const routes = {
  accounts: {
    list: '/member/apps',
    create: '/member/apps/create',
    update: '/member/apps/:id',
  },
  apps: {
    list: '/member/apps',
    create: '/member/apps/create',
    update: '/member/apps/:id',
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
