/** @type {import('tailwindcss').Config} */
const colors = require('tailwindcss/colors');

module.exports = {
  content: ["./src/**/*.{js,jsx,ts,tsx}"],
  theme: {
    colors: {
      primary: "#ed1944",
      primaryHover: "#9f43f0",
      darkPrimary: "#E20039",
      slate: colors.slate,
      white: colors.white,
      blue: colors.blue,
      aqua: colors.aqua,
      black: colors.black,
      gray: colors.gray,
      dark: "#18191a",
      darkHover: "#3c3e40",
      yellow: colors.yellow,
      red: colors.red,
      green: colors.green,
      dimmed: "#ebe8e1",
      lime: colors.lime,
    },
    fontFamily: {
      "default" : ["Ubuntu", "sans"],
      "light" : ["UbuntuLight", "sans"],
      "bold" : ["UbuntuBold", "sans"]
    },
    extend: {},
  },
  plugins: [],
};
