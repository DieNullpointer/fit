import { Autocomplete, TextField, FormControl } from "@mui/material";
import Style from "../../styleConstants";
import styled from "@emotion/styled";

export default function AutoComplete({
  options,
  label,
  id,
  required,
  full,
  onChange,
  defaultValue,
  value
}) {
  let items = [];
  if (!options[0]?.guid)
    options?.map?.((option, idx) => {
      return items.push({ value: idx + 1, text: option });
    });
  else
    options?.map?.((option, idx) => {
      return items.push({ value: option.guid, text: option.text });
    });
  const ColorForm = styled(FormControl)(({ theme }) => ({
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

  return (
    <ColorForm fullWidth={full}>
      <Autocomplete
        options={items}
        getOptionLabel={(option) => option.text}
        id={id}
        autoHighlight
        fullWidth
        onChange={onChange}

        defaultValue={defaultValue}
        value={value}
        isOptionEqualToValue={(option, value) => {
          return option.text === value.text;
        }}
        renderInput={(params) => (
          <TextField
            {...params}
            label={label}
            variant="standard"
            required={required}
          />
        )}
      />
    </ColorForm>
  );
}
