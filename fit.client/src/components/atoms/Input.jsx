import styled from "@emotion/styled";
import { IconButton, TextField } from "@mui/material";
import Style from "../../styleConstants";
import { useState, useRef, useEffect } from "react";

export default function Input({
  adornment,
  required,
  defaultValue,
  label,
  type,
  purpose,
  id,
  onChange,
  size,
  block,
  full,
  disabled,
  className,
  value
}) {

  const ColorTextField = styled(TextField)(({ theme }) => ({
    display: block ? "block" : "inline-block",
    "& label.Mui-focused": {
      color: Style.colors.dark,
    },
    "& .MuiInput-underline:after": {
      borderBottomColor: Style.colors.primary,
    },
    "& .MuiOutlinedInput-root": {
      "& fieldset": {
        borderColor: Style.colors.primary,
      },
      "&:hover fieldset": {
        borderColor: Style.colors.primary,
      },
      "&.Mui-focused fieldset": {
        borderColor: Style.colors.primary,
      },
    },
  }));
  const txf = (
    <ColorTextField
      id={id}
      onChange={onChange}
      label={label}
      required={required}
      type={type || "text"}
      value={value}
      defaultValue={defaultValue}
      InputProps={{
        startAdornment:
          adornment == null ? null : <IconButton>{adornment}</IconButton>,
      }}
      autoComplete={purpose === "password" ? "current-password" : purpose}
      variant="standard"
      fullWidth={full}
      disabled={disabled}
    />
  );
  return className ? <div className={className} children={txf}></div> : txf;
}
