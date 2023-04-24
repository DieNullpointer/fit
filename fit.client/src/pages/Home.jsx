
import PageFrame from "../components/PageFrame";
import { Typography } from "@mui/material";

export default function Home() {
  return (
    <PageFrame active="home" margin>
      <Typography variant="h4">Willkommen</Typography>
      <Typography variant="h6" gutterBottom>
        Homepage folgt
      </Typography>
      <Typography variant="subtitle2">URL für Signup: /signup</Typography>
    </PageFrame>
  );
}
