import { IResultBase } from '@annium/data'
import { useStore } from '@annium/utils/dist/helpers'
import TextField from '@material-ui/core/TextField'
import AutocompleteBase from '@material-ui/lab/Autocomplete'
import { toJS } from 'mobx'
import { observer } from 'mobx-react-lite'
import React, { useCallback, useEffect } from 'react'
import { getLog } from 'shared/utils/log'

import { Store } from './store'


const log = getLog('Autocomplete.View')

type Props<T> = {
  data: T[]
  value: T | null
  label: string
  required?: boolean
  autoCompleteClassName?: string
  inputClassName?: string
  search(query: string): Promise<IResultBase>
  getLabel(value: T): string
  isMatch(value: T, target: T): boolean
  onSelect(value: T | null): void
}

export const Autocomplete = observer(function Autocomplete<T>(props: Props<T>) {
  const {
    data,
    value,
    label,
    required,
    autoCompleteClassName,
    inputClassName,
    search,
    getLabel,
    isMatch,
    onSelect,
  } = props
  const store = useStore(new Store(value, search))

  useEffect(() => {
    log('set value from props')
    store.setQuery(resolveLabel(getLabel, value))
    store.setValue(value)
  }, [store, getLabel, value])

  useEffect(() => {
    log('load after query changed')
    store.load()
  }, [store, store.query])

  const onInputChange = useCallback(handleInputChange(store.setQuery), [store])
  const onChange = useCallback(
    handleChange(getLabel, store.setQuery, store.setValue, onSelect),
    [getLabel, store, onSelect],
  )
  const renderInput = useCallback((params: object) => (
    <TextField
      {...params}
      className={inputClassName}
      required={required}
      label={label}
      fullWidth={true}
      size="small"
      variant="filled"
    />
  ), [inputClassName, required, label])

  log('render:', toJS(data), toJS(value), store.query, store.value)

  return (
    <AutocompleteBase<T>
      className={autoCompleteClassName}
      autoComplete={true}
      options={toJS(data)}
      getOptionLabel={getLabel}
      getOptionSelected={isMatch}
      renderInput={renderInput}
      onInputChange={onInputChange}
      loading={store.state.isLoading}
      onChange={onChange}
      inputValue={store.query}
      value={store.value}
      clearOnBlur={false}
    />
  )
})

function handleInputChange(
  setQuery: (value: string) => void,
) {
  return (event: React.ChangeEvent<{}>, value: string) => {
    setQuery(value)
  }
}

function handleChange<T>(
  getLabel: (value: T) => string,
  setQuery: (value: string) => void,
  setValue: (value: T | null) => void,
  onSelect: (value: T | null) => void,
) {
  return (event: React.ChangeEvent<{}>, value: T | null) => {
    setQuery(resolveLabel(getLabel, value))
    setValue(value)
    onSelect(value)
  }
}

function resolveLabel<T>(get: (value: T) => string, value: T | null): string {
  return value ? get(value) : ''
}
