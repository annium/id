import Box from '@material-ui/core/Box'
import Typography from '@material-ui/core/Typography'
import React from 'react'


export type NotFoundProps = {
  type: string
  id: string
}

export const NotFound = ({ type, id }: NotFoundProps) => (
  <Box mt={3}>
    <Typography component="h1" variant="h5" align="center">
      {type} {id} doesn't exist
    </Typography>
  </Box>
)
