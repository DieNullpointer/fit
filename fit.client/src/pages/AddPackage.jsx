import Input from "../components/atoms/Input";
import Button from "../components/atoms/Button";
import { useState, useRef, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import PageFrame from "../components/PageFrame";
import axios from "axios";
import { Typography } from "@mui/material";
import Style from "../styleConstants";

export default function AddPackage()
{
    const navigate = useNavigate();
    const [name, setName] = useState("");
    const [price, setPrice] = useState("");
    const [validation, setValidation] = useState({});
    let nameRef = useRef(name);
    let priceRef = useRef(price);

    useEffect(() => {
        setName(nameRef.current);
        setPrice(priceRef.current);
    }, []);

    async function handleClick()
    {
        /* await fetch(`https://localhost:5001/api/Package/add`, {
            method: "POST",
            timeout: 5000,
            body: JSON.stringify({
                "name": nameRef.current,
                "price": priceRef.current.replace(".", ","),
            }),
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json",
            },
        }).then((res) => {
            if(res.status === 200) {
                navigate('/admin');
                return res.json();
            }
            if(res.status === 400) {
                console.log(res);
                return res.json();
            }
            //throw new Error(res.status);
        }).catch((err) => console.log(err));
        //.then(data => console.log(data)) */
        

        var model = {name: nameRef.current, price: priceRef.current}
        try {
            await (await axios.post('https://localhost:5001/api/package/add', model))
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
    }

    return(
        <PageFrame>
            <div className="min-h-screen">
                <div className="flex flex-col space-y-4 m-5">
                    <Input
                    label="Package Name"
                    required
                    id="package-name"
                    purpose="package Name"
                    type="text"
                    size={"medium"}
                    onChange={(e) => nameRef.current=e.target.value}
                    value={name}/>
                    <Input
                    label="Package Price"
                    required
                    id="package-price"
                    purpose="package price"
                    type="text"
                    size={"medium"}
                    adornment="€"
                    onChange={(e) => priceRef.current=e.target.value}
                    value={price}/>
                    {validation.name === null ? null : <Typography color={Style.colors.primary} variant="h6">{validation.name}</Typography>}
                    {validation.price === null ? null : <Typography color={Style.colors.primary} variant="h6">{validation.price}</Typography>}
                    <Button text="Senden" sharp onClick={handleClick}/>
                </div>
            </div>
        </PageFrame>
    );
}