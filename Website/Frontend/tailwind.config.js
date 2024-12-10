/** @type {import('tailwindcss').Config} */
module.exports = {
  content: {
    relative: true,
    files: [
      "../Pages/**/*.cshtml",
      "../Areas/**/*.cshtml",
    ]
  },
  darkMode: 'selector',
  safelist: [
    {
      pattern: /bg-(red|green)-(50|100)/,
    },
    {
      pattern: /text-(red|green)-(400|500|800)/,
    },
    {
      pattern: /ring-(red|green)-600/,
    },
    {
      pattern: /ring-offset-(red|green)-50/,
    }
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
