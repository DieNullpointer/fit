import { useEffect, useState } from "react";
import PageFrame from "../components/PageFrame";
import Image from "../components/atoms/Image";

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
      <Image src="Y:\Projekte\fit\Logo">
      </Image>
      <div className="text-center">
        <p>WILLKOMMEN zum FIT 23!</p>
      </div>
    </PageFrame>
  );
}
