export const routes = {
  apps: {
    list: '/member',
    view: '/member/apps/:app',
    my: '/member/apps/my',
    new: '/member/apps/new',
    edit: '/member/apps/:app/edit',
    users: {
      list: '/member/apps/:app/users',
      user: {
        view: '/member/apps/:app/users/:user',
        roles: '/member/apps/:app/users/:user/roles',
        claims: '/member/apps/:app/users/:user/claims',
        companies: '/member/apps/:app/users/:user/companies',
      },
    },
    roles: {
      list: '/member/apps/:app/roles',
      view: '/member/apps/:app/roles/:role',
      new: '/member/apps/:app/roles/:role/new',
      edit: '/member/apps/:app/roles/:role/edit',
    },
  },
  profile: '/member/profile',
}
