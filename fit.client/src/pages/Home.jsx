import { useEffect, useState } from "react";
import Button from "../components/atoms/Button";
import Input from "../components/atoms/Input";
import Select from "../components/atoms/Select";
import PageFrame from "../components/PageFrame";

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
      <div className="flex flex-col space-y-4 m-5">
        <Button text="Button" sharp />
        <Input
          label="username"
          required
          id="in-name"
          purpose="username"
          type="text"
          size={"medium"}
        />
        <Input
          label="password"
          required
          id="in-pw"
          purpose="password"
          type="password"
        />
        <Select options={eventlist?.map((event) => { return event.name + " (" + event.date + ")"}) ?? ["-"]} label="Event" id="event"  />
      </div>
      
      </PageFrame>
  );
}
