import { SnackbarProvider, useSnackbar } from 'notistack'
import React, { ReactNode } from 'react'


type Message = string | React.ReactNode

export const useNotifications = () => {
    const { enqueueSnackbar } = useSnackbar()

    return {
        log: (message: Message) => enqueueSnackbar(message, { variant: 'default' }),
        error: (message: Message) => enqueueSnackbar(message, { variant: 'error' }),
        info: (message: Message) => enqueueSnackbar(message, { variant: 'info' }),
        success: (message: Message) => enqueueSnackbar(message, { variant: 'success' }),
        warn: (message: Message) => enqueueSnackbar(message, { variant: 'warning' }),
    }
}

type NotificationProviderProps = { children: ReactNode }

export const NotificitionsProvider = ({ children }: NotificationProviderProps) => (
    <SnackbarProvider
        maxSnack={1}
        anchorOrigin={{ horizontal: 'center', vertical: 'top' }}
        autoHideDuration={2000}
    >
        {children}
    </SnackbarProvider>
)
