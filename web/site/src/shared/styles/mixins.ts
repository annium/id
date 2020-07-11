import { CSSProperties } from '@material-ui/styles'

export const flexRow = (
  alignItems: CSSProperties['alignItems'] = 'stretch',
  justifyContent: CSSProperties['justifyContent'] = 'flex-start',
): CSSProperties => ({
  display: 'flex',
  flexDirection: 'row',
  alignItems,
  justifyContent,
})

export const flexColumn = (
  alignItems: CSSProperties['alignItems'] = 'stretch',
  justifyContent: CSSProperties['justifyContent'] = 'flex-start',
): CSSProperties => ({
  display: 'flex',
  flexDirection: 'column',
  alignItems,
  justifyContent,
})
