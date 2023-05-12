import PageFrame from "../components/PageFrame";
import { Typography } from "@mui/material";
import { Link } from "react-router-dom";


export default function NotFound()
{
    return(
    <PageFrame active="home" margin>
      <Typography variant="h3" center>404 Not Found</Typography>
      <Typography variant="subtitle2">Back to Homepage<Link to={"/"}> /home</Link></Typography>
    </PageFrame>
    );
}