import { Validator } from 'fluentvalidation-ts'
import { App } from 'member/models/App'

export class DataValidator extends Validator<App> {
  public constructor() {
    super()

    this.ruleFor('name')
      .notEmpty()
      .minLength(2)
  }
}
