import styled from "@emotion/styled";
import { TextField } from "@mui/material";
import Style from "../../styleConstants";

export default function Input({required, value, label, type, purpose, id, onChange, size, block, full }) {
  const ColorTextField = styled(TextField)(({ theme }) => ({
    display: block ? 'block' : 'inline-block',
    '& label.Mui-focused': {
      color: Style.colors.dark,
    },
    '& .MuiInput-underline:after': {
      borderBottomColor: Style.colors.primary,
    },
    '& .MuiOutlinedInput-root': {
      '& fieldset': {
        borderColor: Style.colors.primary,
      },
      '&:hover fieldset': {
        borderColor: Style.colors.primary,
      },
      '&.Mui-focused fieldset': {
        borderColor: Style.colors.primary,
      },
    },
  }));
  
  return (
    <ColorTextField
      id={id}
      onChange={onChange}
      label={label}
      required={required}
      type={type || "text"}
      defaultValue={value}
      autoComplete={purpose === "password" ? "current-password" : purpose}
      variant="standard"
      fullWidth={full}
    />
  );
}
