import PropTypes from "prop-types";
import * as React from 'react';
import { styled } from '@mui/material/styles';
import MButton from '@mui/material/Button';
import Style from "../../styleConstants";

export default function Button({
  onClick,
  id,
  text,
  focused,
  sharp,
  className
}) {
  const ColorButton = styled(MButton)(({ theme }) => ({
    color: theme.palette.getContrastText(Style.colors.primary),
    backgroundColor: Style.colors.primary,
    '&:hover': {
      backgroundColor: Style.colors.dark,
    },
    borderRadius: sharp ? 0 : 4,
  }));
  
  return (
    <div className={className || ""}>
      <ColorButton id={id} onClick={onClick} variant="contained">{text}</ColorButton>
    </div>
  );
}

Button.propTypes = {
  text: PropTypes.string,
  onClick: PropTypes.func,
  focused: PropTypes.string
};

Button.defaultProps = {
  focused: false,
};
