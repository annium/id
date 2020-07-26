export const routes = {
  apps: {
    list: '/member',
    my: '/member/apps/my',
    view: '/member/apps/:app',
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
    companies: {
      list: '/member/apps/:app/companies',
      my: '/member/apps/:app/companies/my',
      view: '/member/apps/:app/companies/:company',
      new: '/member/apps/:app/companies/new',
      edit: '/member/apps/:app/companies/my',
      users: '/member/apps/:app/companies/:company/users',
      roles: '/member/apps/:app/companies/:company/roles',
      claims: '/member/apps/:app/companies/:company/claims',
      companies: '/member/apps/:app/companies/:company/companies',
    },
  },
  profile: '/member/profile',
}
