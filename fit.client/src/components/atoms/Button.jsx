import PropTypes from "prop-types";
import * as React from "react";
import { styled } from "@mui/material/styles";
import MButton from "@mui/material/Button";
import Style from "../../styleConstants";

export default function Button({
  onClick,
  id,
  text,
  focused,
  sharp,
  className,
  loading,
  style,
  submit,
  disabled
}) {
  const ColorButton = styled(MButton)(({ theme }) => ({
    color: theme.palette.getContrastText(Style.colors.primary),
    backgroundColor: Style.colors.primary,
    "&:hover": {
      backgroundColor: Style.colors.dark,
    },
    borderRadius: sharp ? 0 : 4,
  }));

  return (
    <div className={className || ""}>
      <ColorButton style={style} id={id} onClick={onClick} variant="contained" type={submit ? "submit" : "button"} disabled={disabled}>
        {!loading ? (
          text
        ) : (
          <svg
          fill="none"
          className="animate-spin m-1"
          height={"15"}
          viewBox={`0 0 20 20`}
          width="20"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M10 3.5C6.41015 3.5 3.5 6.41015 3.5 10C3.5 10.4142 3.16421 10.75 2.75 10.75C2.33579 10.75 2 10.4142 2 10C2 5.58172 5.58172 2 10 2C14.4183 2 18 5.58172 18 10C18 14.4183 14.4183 18 10 18C9.58579 18 9.25 17.6642 9.25 17.25C9.25 16.8358 9.58579 16.5 10 16.5C13.5899 16.5 16.5 13.5899 16.5 10C16.5 6.41015 13.5899 3.5 10 3.5Z"
            fill="white"
          />
        </svg>
        )}
      </ColorButton>
    </div>
  );
}

Button.propTypes = {
  text: PropTypes.string,
  onClick: PropTypes.func,
  focused: PropTypes.bool,
};

Button.defaultProps = {
  focused: false,
};
