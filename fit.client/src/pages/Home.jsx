
import PageFrame from "../components/PageFrame";
import { Typography } from "@mui/material";

export default function Home() {
  return (
    <PageFrame active="home">
      <div className="flex flex-col space-y-4 m-5">
        <Typography variant="h3">Willkommen</Typography>
      </div>
      
      </PageFrame>
  );
}
