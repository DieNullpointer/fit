import * as React from 'react';
import InputBase from '@mui/material/InputBase';
import { styled } from '@mui/material/styles';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import MuiSelect from '@mui/material/Select';
import Style from "../../styleConstants"


export default function Select({ onChange, value, label, id, options }) {
  let items = [];
  options.map((option, idx) => {
    return items.push({ value: idx + 1, text: option });
  });

  const BootstrapInput = styled(InputBase)(({ theme }) => ({
    '& .MuiInputBase-input': {
      '&:focus': {
        borderColor: Style.colors.primary,
      },
    },
  }));

  return (
    <FormControl fullWidth>
      <InputLabel id="demo-simple-select-label">{label}</InputLabel>
      <MuiSelect
        labelId={`${id}-label`}
        id={id}
        value={value}
        label={label}
        onChange={onChange}
      >
        {items.map((item) => {
          return <MenuItem value={item.value}>{item.text}</MenuItem>;
        })}
      </MuiSelect>
      <BootstrapInput />
    </FormControl>
  );
}
