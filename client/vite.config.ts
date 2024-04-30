import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react-swc'
import { TanStackRouterVite } from '@tanstack/router-vite-plugin'
import checker from 'vite-plugin-checker'
import path from 'path'

// @ts-expect-error mode is not defined
export default ({ mode }) => {
  process.env = { ...process.env, ...loadEnv(mode, process.cwd(), '') }
  const VITE_PORT = process.env.VITE_PORT ?? '3002'
  const VITE_HOST = process.env.VITE_HOST ?? 'localhost'

  const API_URL = process.env.API_URL

  if (API_URL == null) {
    throw new Error('API_URL need to be defined')
  }

  // https://vitejs.dev/config/
  return defineConfig({
    plugins: [
      react(),
      checker({
        typescript: false,
      }),
      // basicSsl(),
      TanStackRouterVite(),
    ],
    server: {
      host: `${VITE_HOST}`,
      port: +`${VITE_PORT}`,
      proxy: {
        '/api': {
          target: API_URL,
          changeOrigin: true,
        },
      },
      watch: { usePolling: true },
    },
    css: {
      transformer: 'lightningcss',
    },
    resolve: {
      alias: {
        '@': path.resolve(__dirname, './src'),
      },
    },
  })
}
