//import { useState } from "react";
import Button from "../components/atoms/Button";
import DatePicker from "../components/atoms/DatePicker";
import Input from "../components/atoms/Input";
import Footer from "../components/Footer";
import Navbar from "../components/Navbar";


export default function AddEvent()
{
    //const [selected, setSelected] = useState(null);
    let name = null;
    let date = null;

    async function handleClick()
    {
        console.log(name);
        console.log(date);
        

        await fetch(`https://localhost:5001/api/Event/add`, {
            method: "POST",
            timeout: 5000,
            body: JSON.stringify({
                "name": name,
                "date": date,
            }),
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json",
            },
        })
        .then((res) => {
            if(res.status === 200) return res.json();
            throw new Error(res.status);
        })
        //.then(data => console.log(data))
        .catch((error) => console.log(error));
    }
    
    return(
        <div>
            <div className="min-h-screen">
                <Navbar pages={[{name: "sign-up"}, {name: "about"}]} profileSettings />
                <div className="flex flex-col space-y-4 m-5">
                    <Input
                    label="Event Name"
                    required
                    id="event-name"
                    purpose="event Name"
                    type="text"
                    size={"medium"}
                    onChange={(e) => name=e.target.value}
                    value={name}
                    />
                    <DatePicker value={date} onchange={(val) => {date=val}}/>
                    <Button text="Button" sharp onClick={handleClick}/>
                </div>
            </div>
            <Footer oldschool />
        </div>
    );
}