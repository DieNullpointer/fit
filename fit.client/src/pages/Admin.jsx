import Navbar from "../components/Navbar";
import Button from "../components/atoms/Button";
import { useNavigate } from "react-router-dom";

export default function Admin()
{
    const navigate = useNavigate();
    return(
        <div>
            <div className="min-h-screen">
                <Navbar pages={[{name: "sign-up"}, {name: "about"}]} profileSettings />
                <div className="flex flex-row space-x-20 justify-center items-center mt-52">
                    <Button text="Event hinzufügen" sharp onClick={() => navigate("/event/add")}/>
                    <Button text="Paket hinzufügen" sharp onClick={() => navigate("/package/add")}/>
                </div>
            </div>
        </div>
    )
}