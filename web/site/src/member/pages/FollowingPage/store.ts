import { createState, resultToStatus, State } from '@annium/forms'
import { AsyncDataState, AsyncState } from '@annium/utils/dist/async'
import { mapResponseArray } from '@annium/utils/dist/helpers'
import { INotificationStore } from '@annium/utils/dist/stores'
import { lazyInject } from 'member/config/di/container'
import { services } from 'member/config/di/services'
import { Subscription, SubscriptionResponseSchema } from 'member/models/Subscription'
import { routes } from 'member/routes'
import { getLog } from 'member/utils/log'
import { action, IObservableValue, observable, runInAction, toJS } from 'mobx'
import { subscriptionService } from 'shared/api/server/subscriptionService'
import { IRouterStore } from 'shared/stores/RouterStore'


export type Data = {
  userId: string
  masterId: string
  followerId: string
}
const log = getLog('FollowingStore')

export class Store {
  @observable
  public subscriptions: AsyncDataState<Subscription[]> = new AsyncDataState<Subscription[]>([])
  @observable
  public form: State<Data> = createState<Data>({
    userId: '',
    masterId: '',
    followerId: '',
  })
  @observable
  public state: AsyncState = new AsyncState()
  @observable
  public deleteCandidate: IObservableValue<string | null> = observable.box(null)
  @lazyInject(services.RouterStore)
  public router!: IRouterStore
  @lazyInject(services.NotificationStore)
  public notifications!: INotificationStore

  public constructor() {
    delete this.notifications
    delete this.router
  }

  @action.bound
  public async load() {
    this.subscriptions.start()

    const result = await subscriptionService.getFollowerSubscriptions()
      .then(mapResponseArray(SubscriptionResponseSchema))

    if (result.isSuccess)
      this.subscriptions.success(result.data)
    else
      this.subscriptions.failure(result)
  }


  @action.bound
  public async create() {
    this.state.start()

    const result = await subscriptionService.createSubscription(toJS(this.form.value))

    if (result.isSuccess) {
      this.state.success()
      this.form.reset()

      await this.load()

      this.router.go(routes.following.list)

      return
    }

    this.state.failure(result)

    this.form.setStatus(resultToStatus(result))
    if (result.plainErrors.length)
      this.notifications.error(result.plainErrors.join(', '))
    else if (result.labeledErrors.user)
      this.notifications.error(result.labeledErrors.user.join(', '))
  }

  @action.bound
  public setDeleteCandidate(candidate: string | null) {
    return () => runInAction(() => {
      this.deleteCandidate.set(candidate)
    })
  }

  @action.bound
  public async delete() {
    if (!this.deleteCandidate) {
      log('No delete candidate')

      return
    }

    this.subscriptions.start()

    const id = this.deleteCandidate.get()!
    this.deleteCandidate.set(null)

    const result = await subscriptionService.deleteSubscription(id)

    if (result.isFailure) {
      this.subscriptions.success(this.subscriptions.data)

      if (result.plainErrors.length)
        this.notifications.error(result.plainErrors.join(', '))

      return
    }

    await this.load()
  }
}
