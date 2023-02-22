import { useState } from "react";
import Menu from "../components/Menu";
import Navbar from "../components/Navbar";
import Footer from "../components/Footer";

export default function Packages()
{
    const [namelist, setNamelist] = useState();
    //const [selectedpackage]

    async function fetchAllEvents()
    {
    await fetch(`https://localhost:5001/api/Package`)
      .then((res) => res.json())
      .then((data) => {
        console.log(data);
        setNamelist(data);
      })
      .catch((error) => console.log(error));
    }

    return(
        <div>
            <div className="min-h-screen">
                <Navbar pages={[{name: "sign-up"}, {name: "about"}]} profileSettings />
                <Menu />
            </div>
            <Footer oldschool />
        </div>
    );
}