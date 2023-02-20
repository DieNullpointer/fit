import MuiPaper from "@mui/material/Paper"
import { styled } from "@mui/material";

export default function Paper({elevation, children, center, id}) {
    const Item = styled(MuiPaper)(({ theme }) => ({
        ...theme.typography.body2,
        textAlign: center ? 'center' : 'left',
        margin: '10px',
        lineHeight: '60px',
      }));

    return <Item elevation={elevation}>{children}</Item>
}