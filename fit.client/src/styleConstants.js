const { createTheme } = require("@mui/material");
const tColors = require("tailwindcss/colors");

class Style {
  static colors = {
    primary: "#ed1944",
    primaryHover: "#d40b33",
    darkPrimary: "#c70234",
    slate: tColors.slate,
    red: tColors.red,
    white: tColors.white,
    blue: tColors.blue,
    aqua: tColors.aqua,
    black: tColors.black,
    gray: tColors.gray,
    dark: "#18191a",
    darkHover: "#3c3e40",
    yellow: tColors.yellow,
    green: tColors.green,
    dimmed: "#ebe8e1",
    lime: tColors.lime,
  };
  static theme = createTheme({
    typography: {
      fontFamily: "Ubuntu",
    },
    components: {
      MuiCssBaseline: {
        styleOverrides: `
          @font-face {
            font-family: "Ubuntu";
            src: url("../public/fonts/Ubuntu-Regular.ttf"), format('ttf');
          }      
        `,
      },
    },
  });
}

export default Style;
