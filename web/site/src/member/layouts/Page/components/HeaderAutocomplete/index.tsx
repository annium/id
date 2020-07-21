import { IResultBase } from '@annium/data'
import { observer } from 'mobx-react-lite'
import React, { useCallback } from 'react'
import { Autocomplete } from 'shared/components/Autocomplete'

type Named = { name: string }
type Props<T extends Named> = {
  className: string
  label: string
  data: T[]
  value: T | null
  set(value: T | null): void
  search(query: string): Promise<IResultBase>
}

export const HeaderAutocomplete = observer(function HeaderAutocomplete<T extends Named>(props: Props<T>) {
  const { className, label, data, value, set, search } = props

  const getLabel = useCallback((x: T) => x.name, [])
  const isMatch = useCallback((x: T, target: T) => x.name === target.name, [])

  return (
    <Autocomplete<T>
      data={data}
      value={value}
      label={label}
      inputClassName={className}
      search={search}
      getLabel={getLabel}
      isMatch={isMatch}
      onSelect={set}
    />
  )
})
