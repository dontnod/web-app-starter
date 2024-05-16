import { loginRedirect } from '@/hooks/use-log-in'
import { AppRouterContext } from '@/routes/__root'
import { redirect } from '@tanstack/react-router'
import { filter, firstValueFrom } from 'rxjs'

/**
 * This function must be used in the {@link @tanstack/react-router#FileBaseRouteOptions.beforeLoad | beforeLoad property of a route}. property of a route in order
 * to guarantee that the user is authenticated, otherwise an identification popup will be presented
 *
 * @throws {@link Redirect}
 * This exception is thrown if the authentication failed and the user needs to be redirected to the home
 */
export async function authenticateGuard(context: AppRouterContext) {
  await firstValueFrom(context.isMsalReady.pipe(filter((isReady) => isReady === true)))

  if (!context.msalInstance.getActiveAccount()) {
    // If the user is not authenticated, we try to authenticate him
    try {
      await loginRedirect(context.msalInstance)
    } catch (err) {
      console.error(err)
      throw redirect({
        to: '/',
      })
    }
  }
}
