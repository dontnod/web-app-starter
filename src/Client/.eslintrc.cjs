module.exports = {
  root: true,
  env: { browser: true, es2020: true },
  extends: [
    'eslint:recommended',
    'plugin:@typescript-eslint/stylistic-type-checked',
    'plugin:react-hooks/recommended',
    'plugin:prettier/recommended',
  ],
  parser: '@typescript-eslint/parser',
  "parserOptions": {
    "project": ["tsconfig.json"]
  },
  plugins: ['react-refresh'],
  rules: {
    'react-hooks/rules-of-hooks':'off',
    'no-unused-vars': 'off',
    '@typescript-eslint/no-floating-promises': 'error',
    '@typescript-eslint/no-explicit-any': 'error',
    '@typescript-eslint/no-unused-vars': 'error',
    '@typescript-eslint/no-unnecessary-condition':'error',
    'react-refresh/only-export-components': [
      'warn',
      { allowConstantExport: true },
    ],
    'prettier/prettier': [
      'error',
      {
        "endOfLine": "auto"
      },
    ],
  },
  ignorePatterns: ['vite.config.ts', '.eslintrc.cjs', 'node_modules', 'dist', '*schema.d.ts', 'routeTree.gen.ts'],
}
