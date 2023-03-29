import { useEffect, useState } from "react";
import PageFrame from "../components/PageFrame";
import { Typography } from "@mui/material";
import { Box } from "@mui/system";
import Carousel, {CarouselItem} from "../components/Carousel";
import Image from "../components/atoms/Image";
import Logo from "../components/atoms/Logo";

export default function Home() {
  const [eventlist, setEventlist] = useState();
  useEffect(() => {
    init();
  });

  async function fetchAllEvents() {
    await fetch(`https://localhost:5001/api/Event`)
      .then((res) => res.json())
      .then((data) => {
        console.log(data);
        setEventlist(data);
      })
      .catch((error) => console.log(error));
  }

  async function init() {
    await fetchAllEvents();
  }

  return (
    <PageFrame active="home">
      <Box sx={{ color: "dark", textAlign: "center" }}>
        <Typography variant="h4">Willkommen zum FIT 23!</Typography>
        <Typography variant="subtitle1" gutterBottom>
          Firmeninformationstag
        </Typography>
      </Box>
      <div className="app">
        <Carousel>
          <CarouselItem>  
            <Image src="../../titlepicture.png"></Image>
          </CarouselItem>
          <CarouselItem>  
            <Image src="../../logo.png"></Image>
          </CarouselItem>
        </Carousel>
      </div>
    </PageFrame>
  );
}
