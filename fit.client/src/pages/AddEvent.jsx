import { useState, useRef, useEffect } from "react";
import Button from "../components/atoms/Button";
import DatePicker from "../components/atoms/DatePicker";
import Input from "../components/atoms/Input";
import Footer from "../components/Footer";
import Navbar from "../components/Navbar";
import axios from "axios";
import { Typography } from "@mui/material";
import Style from "../styleConstants";


export default function AddEvent()
{
    const [selected, setSelected] = useState(Date.now());
    const [name, setName] = useState("");
    const [validation, setValidation] = useState({});
    //let name = null;
    let nameRef = useRef(name);
    let selectedRef = useRef(selected);
    //let date = null;

    useEffect(() => {
        setSelected(selectedRef.current);
        setName(nameRef.current)
    }, []);

    function handleChange(value)
    {
        selectedRef.current = value;
    }

    async function handleClick()
    {
        console.log(validation);
        console.log(nameRef.current);
        console.log(selectedRef.current);
        //console.log(selectedRef);
        var model = {name: nameRef.current, date: selectedRef.current.toJSON()}
        try {
            await (await axios.post('event/add', model))
        } catch(e) {
            if(e.response.status === 400)
            {
                console.log(e.response.data.errors);
                setValidation(Object.keys(e.response.data.errors).reduce((prev, key) => {
                    const newKey = key.charAt(0).toLowerCase() + key.slice(1);
                    prev[newKey] = e.response.data.errors[key][0];
                    return prev;
                }, {}));
                console.log(validation);
            }
        }

        /* await fetch(`https://localhost:5001/api/Event/add`, {
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
            if(res.status === 200) {
                navigate('/admin');
                return res.json();
            }
            
        })
        //.then(data => console.log(data))
        .catch((error) => console.log(error)); */
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
                    {validation.name === null ? null : <Typography color={Style.colors.primary} variant="subtitle1">{validation.name}</Typography>}
                    <DatePicker value={selected} accepted={(val) => setSelected(val)} onchange={(val) => handleChange(val)}/>
                    <Button text="Senden" sharp onClick={handleClick}/>
                </div>
            </div>
            <Footer oldschool />
        </div>
    );
}