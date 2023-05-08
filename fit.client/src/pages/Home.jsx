
import PageFrame from "../components/PageFrame";
import { Typography } from "@mui/material";
import { Link } from "react-router-dom";

export default function Home() {
  return (
    <PageFrame active="home" margin>
      <Typography variant="h4">Willkommen</Typography>
      <Typography variant="h6" gutterBottom>
        Homepage folgt
      </Typography>
      <Typography variant="subtitle2">URL für Signup: <Link to={"/signup"}>/signup</Link></Typography>
    </PageFrame>
  );
}
