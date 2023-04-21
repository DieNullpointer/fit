import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Button from "../components/atoms/Button";
import Input from "../components/atoms/Input";
import Select from "../components/atoms/Select";
import PageFrame from "../components/PageFrame";
import { Typography } from "@mui/material";

export default function Home() {
  const [eventlist, setEventlist] = useState();
  const url = useParams();
  useEffect(() => {
    init();
  }, [url]);

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
    <PageFrame active="home" margin>
      <Typography variant="h4">Willkommen</Typography>
      <Typography variant="h6" gutterBottom>
        Homepage folgt
      </Typography>
      <Typography variant="subtitle2">URL für Signup: /signup</Typography>
    </PageFrame>
  );
}
