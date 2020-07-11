import { Errors, errorsToStatus, State, StateStatus, Status } from '@annium/forms'
import { Validator } from 'fluentvalidation-ts'

export const readAutocompleteValue = <T extends Object, K extends keyof T>(field: K, defaultValue: T[K]) =>
  (value: T): T[K] => value ? value[field] : defaultValue


export const validate = <T>(ValidatorClass: new() => Validator<T>) => (state: State<T>): void => {
  state.setStatus(Status.Validating)
  const errors: Errors<T> = new ValidatorClass().validate(state.value)
  const status: StateStatus<T> = errorsToStatus(errors)

  state.setStatus(status, Status.Success)
}
