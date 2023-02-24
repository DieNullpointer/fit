import MuiPaper from "@mui/material/Paper"
import { styled } from "@mui/material";

export default function Paper({elevation, children, center, id, className}) {
    const Item = styled(MuiPaper)(({ theme }) => ({
        ...theme.typography.body2,
        textAlign: center ? 'center' : 'left',
        margin: '10px',
        lineHeight: '60px',
      }));
      const Export = <Item id={id} elevation={elevation}>{children}</Item>;

    return !className ? Export : <div className={className}>{Export}</div>
}