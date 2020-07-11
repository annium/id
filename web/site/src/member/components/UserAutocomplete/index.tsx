import { useStore } from '@annium/utils/dist/helpers'
import TextField from '@material-ui/core/TextField'
import Autocomplete from '@material-ui/lab/Autocomplete'
import { RenderInputParams } from '@material-ui/lab/Autocomplete/Autocomplete'
import { User } from 'member/models/User'
import { toJS } from 'mobx'
import { observer } from 'mobx-react-lite'
import React, { useEffect, useState } from 'react'

import { Store } from './store'


type Props = {
  label: string
  required?: boolean
  onChange?(...args: unknown[]): void;
}

export const UserAutocomplete = observer((props: Props) => {
  const store = useStore(new Store())

  const [query, setQuery] = useState<string>('')
  const [value, setValue] = useState<User | null>(null)

  useEffect(() => {
    store.load(query)
  }, [store, query])

  return (
    <Autocomplete
      options={toJS(store.users.data)}
      getOptionLabel={getOptionLabel}
      getOptionSelected={getOptionSelected}
      renderInput={Input(props)}
      onInputChange={handleInputChange(setQuery)}
      onChange={handleChange(props.onChange!, setValue, setQuery)}
      inputValue={query}
      value={value}
    />
  )
})

const Input = ({ label, required }: Props) => (params: RenderInputParams) => (
  <TextField
    {...params}
    required={required}
    label={label}
    fullWidth={true}
  />
)


function getOptionLabel(user: User | null): string {
  return user ? user.login : ''
}

function getOptionSelected(user: User, value: User): boolean {
  return user.login === value.login
}

function handleInputChange(
  setQuery: (value: string) => void,
) {
  return (event: React.ChangeEvent<{}>, value: string) => {
    setQuery(value)
  }
}

function handleChange(
  onChange: (...args: unknown[]) => void,
  setValue: (value: User | null) => void,
  setQuery: (value: string) => void,
) {
  return (event: React.ChangeEvent<{}>, value: User | null) => {
    onChange(value)
    setValue(value!)
    setQuery(value ? value.login : '')
  }
}
