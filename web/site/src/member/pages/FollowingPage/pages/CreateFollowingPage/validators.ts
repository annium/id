import { Validator } from 'fluentvalidation-ts'

import { Data } from '../../store'


export class DataValidator extends Validator<Data> {
  public constructor() {
    super()

    this.ruleFor('userId')
      .notEmpty()

    this.ruleFor('masterId')
      .notEmpty()

    this.ruleFor('followerId')
      .notEmpty()
  }
}
