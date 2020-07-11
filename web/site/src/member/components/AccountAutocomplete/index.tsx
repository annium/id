import { useStore } from '@annium/utils/dist/helpers'
import TextField from '@material-ui/core/TextField'
import Autocomplete from '@material-ui/lab/Autocomplete'
import { RenderInputParams } from '@material-ui/lab/Autocomplete/Autocomplete'
import { toJS } from 'mobx'
import { observer } from 'mobx-react-lite'
import React, { useEffect, useState } from 'react'

import { PublicAccount } from '../../models/PublicAccount'

import { Store } from './store'


type Props = {
  userId: string
  label: string
  required?: boolean
  onChange?(...args: unknown[]): void;
}

export const AccountAutocomplete = observer((props: Props) => {
  const store = useStore(new Store())
  const userId = props.userId

  const [query, setQuery] = useState<string>('')
  const [value, setValue] = useState<PublicAccount | null>(null)

  useEffect(() => {
    store.load(userId)
  }, [store, userId])

  return (
    <Autocomplete
      options={toJS(store.accounts.data)}
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


function getOptionLabel(account: PublicAccount | null): string {
  return account ? account.name : ''
}

function getOptionSelected(account: PublicAccount, value: PublicAccount): boolean {
  return account.name === value.name
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
  setValue: (value: PublicAccount | null) => void,
  setQuery: (value: string) => void,
) {
  return (event: React.ChangeEvent<{}>, value: PublicAccount | null) => {
    onChange(value)
    setValue(value ? value : null)
    setQuery(value ? value.name : '')
  }
}
