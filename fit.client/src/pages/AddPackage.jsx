import Footer from "../components/Footer";
import Navbar from "../components/Navbar";
import Input from "../components/atoms/Input";
import Button from "../components/atoms/Button";
import { useState, useRef, useEffect } from "react";

export default function AddPackage()
{
    const [name, setName] = useState("");
    const [price, setPrice] = useState("");
    let nameRef = useRef(name);
    let priceRef = useRef(price);

    useEffect(() => {
        setName(nameRef.current);
        setPrice(priceRef.current);
    }, []);

    async function handleClick()
    {
        await fetch(`https://localhost:5001/api/Package/add`, {
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
                    <Button text="Senden" sharp onClick={handleClick}/>
                </div>
            </div>
            <Footer oldschool />
        </div>
    );
}