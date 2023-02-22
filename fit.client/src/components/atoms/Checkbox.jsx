import {
  Checkbox as CheckboxMui,
  FormControlLabel,
} from "@mui/material";
import Style from "../../styleConstants";

export default function Checkbox({ label, defaultChecked, onChange}) {
  const label1 = { inputProps: { "aria-label": label } };
  return (
      <FormControlLabel
        control={
          <CheckboxMui
            defaultChecked={defaultChecked}
            {...label1}
            sx={{
              color: Style.colors.primary,
              "&.Mui-checked": { color: Style.colors.primary },
            }}
            onChange={onChange}
          />
        }
        label={label}
      />
  );
}
