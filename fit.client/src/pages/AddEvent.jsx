import { useState, useRef, useEffect } from "react";
import Button from "../components/atoms/Button";
import DatePicker from "../components/atoms/DatePicker";
import Input from "../components/atoms/Input";
import Footer from "../components/Footer";
import Navbar from "../components/Navbar";


export default function AddEvent()
{
    const [selected, setSelected] = useState(Date.now());
    const [name, setName] = useState("");
    //let name = null;
    let nameRef = useRef(name);
    let selectedRef = useRef(selected);
    //let date = null;

    useEffect(() => {
        setSelected(selectedRef.current);
        setName(nameRef.current)
    }, [selected]);

    function handleChange(value)
    {
        selectedRef.current = value;
    }

    async function handleClick()
    {
        console.log(nameRef.current);
        console.log(selectedRef.current);
        //console.log(selectedRef);
        

        await fetch(`https://localhost:5001/api/Event/add`, {
            method: "POST",
            timeout: 5000,
            body: JSON.stringify({
                "name": nameRef.current,
                "date": selectedRef.current.toJSON(),
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
                    onChange={(e) => nameRef.current=e.target.value}
                    value={name}
                    />
                    <DatePicker value={selected} accepted={(val) => setSelected(val)} onchange={(val) => handleChange(val)}/>
                    <Button text="Senden" sharp onClick={handleClick}/>
                </div>
            </div>
            <Footer oldschool />
        </div>
    );
}