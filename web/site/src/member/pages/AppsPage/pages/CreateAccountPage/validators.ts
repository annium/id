import { Validator } from 'fluentvalidation-ts'
import { Account } from 'member/models/Account'

export class DataValidator extends Validator<Account> {
  public constructor() {
    super()

    this.ruleFor('name')
      .notEmpty()
      .minLength(2)

    this.ruleFor('key')
      .notEmpty()
      .minLength(2)

    this.ruleFor('secret')
      .notEmpty()
      .minLength(2)
  }
}
