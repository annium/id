import { Validator } from 'fluentvalidation-ts'

import { Data } from './store'


export class DataValidator extends Validator<Data> {
  public constructor() {
    super()

    this.ruleFor('email')
      .notEmpty()
      .emailAddress()
      .minLength(3)
      .maxLength(100)
  }
}
