import { TextField } from "@mui/material";

export default function Input({required, value, label, type, purpose, id, onChange, size }) {
  return (
    <TextField
      id={id}
      onChange={onChange}
      label={label}
      required={required}
      type={type ||"text"}
      defaultValue={value}
      autoComplete={purpose === "password" ? "current-password" : purpose}
      variant="standard"
      size={size || "full"}
    />
  );
}
