import Box from '@material-ui/core/Box'
import Typography from '@material-ui/core/Typography'
import { MinimalPage } from 'public/layouts/MinimalPage'
import React from 'react'


export const NotFoundPage = () => (
  <MinimalPage title="404">
    <Box mt={3}>
      <Typography component="h1" variant="h5" align="center">
        Page, you are looking for, is not found
    </Typography>
    </Box>
  </MinimalPage>
)
