// import { useEffect, useState } from "react";

export default function Home() {
    /* const [event, setEvent] = useState();

    useEffect(() => {
        init();
    }, []);

    async function init()
    {
        await fetch(`https://localhost:5001/api/event/now`)
            .then((res) => res.json())
            .then((data) => {
                setEvent(data);
                console.log(data);
            })
            .catch((error) => console.log(error));
    } */

    return (
        <div>
            <h1 className="text-center mt-10 font-bold text-primary underline text-3xl">Willkommen zum Fit-Anmeldesystem</h1>
            {/* <h2 className="text-center mt-5 font-bold text-2xl">{event?.name}</h2> */}
        </div>
    );
}