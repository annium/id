import { Status } from '@annium/forms'
import { convert, Field, Form } from '@annium/forms-react'
import Button from '@material-ui/core/Button'
import Grid from '@material-ui/core/Grid'
import Paper from '@material-ui/core/Paper'
import { AccountAutocomplete } from 'member/components/AccountAutocomplete'
import { BreadcrumbItems, Breadcrumbs } from 'member/components/Breadcrumbs'
import { UserAutocomplete } from 'member/components/UserAutocomplete'
import { useInjection } from 'member/config/di'
import { services } from 'member/config/di/services'
import { CentricGrid } from 'member/layouts/CentricGrid'
import { PublicAccount } from 'member/models/PublicAccount'
import { User } from 'member/models/User'
import { routes } from 'member/routes'
import { observer } from 'mobx-react-lite'
import React, { useEffect } from 'react'
import { Link } from 'shared/components/Link'
import { Loader } from 'shared/components/Loader'
import { IMeStore } from 'shared/stores/MeStore'
import { readAutocompleteValue, validate } from 'shared/utils/forms'

import { Store } from '../../store'

import { useStyles } from './styles'
import { DataValidator } from './validators'


export type CreateFollowingPageProps = { store: Store }

export const CreateFollowingPage = observer(({ store }: CreateFollowingPageProps) => {
  const me = useInjection<IMeStore>(services.MeStore)
  const form = store.form
  const state = store.state
  const classes = useStyles()
  const breadcrumbs: BreadcrumbItems = {
    Following: routes.following.list,
    Create: null,
  }

  const isFormSubmittable = !state.isLoading && form.hasStatus(Status.Success) &&
    form.masterId.value && form.followerId.value

  useEffect(() => () => form.reset(), [form])

  return (
    <CentricGrid>
      <Breadcrumbs items={breadcrumbs} />
      <Loader direction="column" align="stretch" justify="flex-start" isLoading={state.isLoading}>
        <Form state={form} onChange={validate(DataValidator)}>
          <Paper className={classes.paper}>
            <Grid container={true} spacing={3} justify="space-between" alignItems="flex-end">
              <Grid item={true} xs={12} sm={6}>
                <Field<string, User, string>
                  state={form.userId}
                  fieldToState={readAutocompleteValue('id', '')}
                  stateToField={convert.asIs}
                >
                  <UserAutocomplete label="Target User" />
                </Field>
              </Grid>
              <Grid item={true} xs={12} sm={6}>
                <Field<string, PublicAccount, string>
                  state={form.masterId}
                  fieldToState={readAutocompleteValue('id', '')}
                  stateToField={convert.asIs}
                >
                  <AccountAutocomplete userId={form.userId.value} label="Target Account" />
                </Field>
              </Grid>
              <Grid item={true} xs={12} sm={6}>
                <Field<string, PublicAccount, string>
                  state={form.followerId}
                  fieldToState={readAutocompleteValue('id', '')}
                  stateToField={convert.asIs}
                >
                  <AccountAutocomplete userId={me.user!.id} label="My Account" />
                </Field>
              </Grid>
              <Grid className={classes.buttons} item={true} xs={12}>
                <Button
                  disableElevation={true}
                  variant="contained"
                  color="primary"
                  disabled={!isFormSubmittable}
                  onClick={store.create}
                >
                  Create
                </Button>
                <Link to={routes.following.list} underline="none">
                  <Button
                    disableElevation={true}
                    variant="contained"
                    color="default"
                  >
                    Cancel
                  </Button>
                </Link>
              </Grid>
            </Grid>
          </Paper>
        </Form>
      </Loader>
    </CentricGrid>
  )
})
