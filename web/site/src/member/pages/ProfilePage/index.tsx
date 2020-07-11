import { BreadcrumbItems, Breadcrumbs } from 'member/components/Breadcrumbs'
import { CentricGrid } from 'member/layouts/CentricGrid'
import { Page } from 'member/layouts/Page'
import { observer } from 'mobx-react-lite'
import React from 'react'

import { AccountForm } from './AccountForm'
import { PasswordForm } from './PasswordForm'

export const ProfilePage = observer(() => {
  const breadcrumbs: BreadcrumbItems = {
    Profile: null,
  }

  return (
    <Page>
      <CentricGrid>
        <Breadcrumbs items={breadcrumbs} />
        <AccountForm />
        <PasswordForm />
      </CentricGrid>
    </Page>
  )
})
