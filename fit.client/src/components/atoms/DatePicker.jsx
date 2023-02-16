import { DesktopDatePicker } from '@mui/x-date-pickers/DesktopDatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import styled from "@emotion/styled";
import { TextField } from "@mui/material";
import Style from "../../styleConstants";
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns'

export default function DatePicker({onchange, value, accepted})
{
    const ColorTextField = styled(TextField)(({ theme }) => ({
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
    return(
        <div>
            <LocalizationProvider dateAdapter={AdapterDateFns}>
                <DesktopDatePicker
                label="Datum" 
                inputFormat='dd-MM-yyyy' 
                value={value} 
                onChange={onchange}
                onAccept={accepted}
                disablePast
                di
                renderInput={(params) => <ColorTextField {...params} />}/>
            </LocalizationProvider>
            
        </div>
    );
}