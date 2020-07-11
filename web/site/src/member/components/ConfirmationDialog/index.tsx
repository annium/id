import Button from '@material-ui/core/Button'
import Dialog from '@material-ui/core/Dialog'
import DialogActions from '@material-ui/core/DialogActions'
import DialogContent from '@material-ui/core/DialogContent'
import DialogContentText from '@material-ui/core/DialogContentText'
import DialogTitle from '@material-ui/core/DialogTitle'
import React, { ReactNode } from 'react'


type Props = {
  isOpen: boolean
  title: string
  children: ReactNode
  onConfirm(): void
  onCancel(): void
}

export const ConfirmationDialog = ({ isOpen, title, children, onConfirm, onCancel }: Props) => {
  if (!isOpen)
    return null

  return (
    <Dialog open={true} onClose={onCancel}>
      <DialogTitle>{title}</DialogTitle>
      <DialogContent>
        <DialogContentText>
          {children}
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={onConfirm} color="secondary" autoFocus={true}>
          Confirm
        </Button>
        <Button onClick={onCancel} color="default">
          Cancel
        </Button>
      </DialogActions>
    </Dialog>
  )
}
