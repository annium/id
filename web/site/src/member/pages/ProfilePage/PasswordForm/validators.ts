import { Validator } from 'fluentvalidation-ts'

import { Data } from './store'


export class DataValidator extends Validator<Data> {
  public constructor() {
    super()

    this.ruleFor('password')
      .notEmpty()
      .minLength(8)
      .maxLength(50)
  }
}
