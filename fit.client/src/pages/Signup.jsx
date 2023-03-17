import React from "react";
import { Typography } from "@mui/material";
import { Box } from "@mui/system";
import Paper from "../components/atoms/Paper";
import PageFrame from "../components/PageFrame";
import Input from "../components/atoms/Input";
import AutoComplete from "../components/atoms/AutoComplete";
import Checkbox from "../components/atoms/Checkbox";
import { useState, useEffect, useRef } from "react";
import Button from "../components/atoms/Button";
import Form from "../components/form/Form";
import APIConstants from "../apiConstants";

let personDivChildrenLength = 0;

export default function Signup() {
  const person = (
    <Paper elevation={3} id="person-paper" key={"paper-key-person-" + (personDivChildrenLength++)} >
      <Form.Section className="px-3">
        <div className="grid md:grid-cols-3 grid-flow-column">
          <Input
            id="in-title"
            label="Titel"
            required
            purpose={"text"}
            full
            className="m-3"
            {...Form.Child("input", "title")}
          />
          <Input
            id="in-firstname"
            label="Vorname"
            required
            purpose={"text"}
            full
            className="m-3"
            {...Form.Child("input", "firstname")}
          />
          <Input
            id="in-lastname"
            label="Nachname"
            required
            purpose={"text"}
            full
            className="m-3"
            {...Form.Child("input", "lastname")}
          />
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2">
          <div className="pr-3">
            <Input
              id="in-telnr"
              label="Telefonnummer"
              required
              purpose={"text"}
              full
              {...Form.Child("input", "telnr")}
            />
            <Input
              id="in-mobilnr"
              label="Mobilnummer"
              required
              purpose={"text"}
              full
              {...Form.Child("input", "mobilnr")}
            />
            <Input
              id="in-email"
              label="Email"
              required
              purpose={"text"}
              full
              {...Form.Child("input", "email")}
            />
          </div>
          <div>
            <Input
              id="in-function"
              label="Funktion i. d. Firma"
              required
              purpose={"text"}
              full
              {...Form.Child("input", "function")}
            />
          </div>
        </div>
      </Form.Section>
    </Paper>
  );

  const [payDisabled, setPayDisabled] = useState(false);
  const [events, setEvents] = useState([]);
  const [packages, setPackages] = useState([]);
  const [step, setStep] = useState(1);
  const [personDivChildren, setPersonDivChildren] = useState([person]);
  const personDivRef = useRef();

  useEffect(() => {
    init();
  }, []);

  async function init() {
    let newEvents = [];
    let resEvents = await APIConstants.getAllEvents();
    resEvents.map((event) => {
      return newEvents.push({
        text: `${event.name} (${event.date})`,
        guid: event.guid,
      });
    });
    setEvents(newEvents);

    let newPackages = [];
    let resPackages = await APIConstants.getAllPackages();
    resPackages.map((p) => {
      return newPackages.push({
        text: `${p.name} (${p.price}€)`,
        guid: p.guid,
      });
    });
    setPackages(newPackages);
  }

  

  return (
    <PageFrame active={"sign-up"} margin className="bg-primary" noFullScreen>
      <Box sx={{ color: "white", textAlign: "center" }}>
        <Typography variant="h4">Anmeldung für den FIT</Typography>
        <Typography variant="subtitle1" gutterBottom>
          Dieses Formular dient zur Anmeldung zukünfitger FITs
        </Typography>
      </Box>
      <Paper elevation={3} className="mt-12 mb-24 relative">
        <Form.Body className="py-4 px-8">
          <div
            className={`transition ease-in-out duration-700 ${
              step === 2 ? "hidden" : "block"
            }`}
          >
            <div className="w-full">
              <Typography variant="h6">Firmendetails</Typography>
              <div className="pl-3 w-full">
                <div className="mt-2 border-l border-l-dark px-4 py-2 w-full">
                  <div className="grid grid-cols-1 md:grid-cols-2">
                    <Form.Section className="md:pr-3">
                      <Input
                        id="in-comp-name"
                        label="Firmenname"
                        purpose={"text"}
                        required
                        block
                        full
                        {...Form.Child("input", "name")}
                      />
                      <Input
                        id="in-comp-address"
                        label="Firmenadresse"
                        purpose={"text"}
                        required
                        block
                        full
                        {...Form.Child("input", "address")}
                      />
                      <Input
                        id="in-city"
                        label="Ort"
                        purpose={"text"}
                        required
                        block
                        full
                        {...Form.Child("input", "place")}
                      />
                    </Form.Section>
                    <Form.Section className="md:pl-3">
                      <div className="grid grid-cols-3">
                        <Input
                          id="in-comp-zip"
                          label="PLZ"
                          purpose="text"
                          required
                          {...Form.Child("input", "plz")}
                        />
                        <div className="col-span-2 ml-4">
                          <AutoComplete
                            id="in-country"
                            label="Land"
                            options={["Österreich", "Deutschland", "Schweiz"]}
                            required
                            full
                            {...Form.Child("autocomplete", "country")}
                          />
                        </div>
                      </div>
                      <Input
                        id="in-comp-payaddress"
                        label="Rechnungsadresse"
                        purpose={"text"}
                        required
                        disabled={payDisabled}
                        block
                        full
                        {...Form.Child("input", "billAddress")}
                      />
                      <div className="-mt-6">
                        <Checkbox
                          label="Gleich wie Firmenadresse"
                          onChange={(e) => setPayDisabled(e.target.checked)}
                        />
                      </div>
                    </Form.Section>
                  </div>
                </div>
              </div>
            </div>
            <div className="w-full">
              <Typography variant="h6">Paketauswahl</Typography>
              <div className="pl-3 w-full">
                <div className="mt-2 border-l border-l-dark px-4 py-2 w-full">
                  <Form.Section className="grid grid-cols-2">
                    <AutoComplete
                      id="in-fit"
                      options={events}
                      full
                      label="FIT"
                      required
                      {...Form.Child("autocomplete", "event", (e) => {
                        const text = e.target.innerText;
                        let correctGuid = 1;
                        events.forEach((ev) => {
                          if (ev.text === text) return (correctGuid = ev.guid);
                        });
                        return correctGuid;
                      })}
                    />
                    <div className="md:ml-3">
                      <AutoComplete
                        id="in-package"
                        options={packages}
                        full
                        label="Paket"
                        required
                        {...Form.Child("autocomplete", "package", (e) => {
                          const text = e.target.innerText;
                          let correctGuid = 1;
                          packages.forEach((p) => {
                            if (p.text === text) return (correctGuid = p.guid);
                          });
                          return correctGuid;
                        })}
                      />
                    </div>
                  </Form.Section>
                </div>
              </div>
            </div>
          </div>
          <div
            className={`transition ease-in-out duration-200 ${
              step === 1 ? "hidden" : "block"
            }`}
          >
            <div className="w-full">
              <Typography variant="h6">Ansprechpartner</Typography>
              <div className="pl-3 w-full">
                <div className="mt-2 border-l border-l-dark px-4 py-2 w-full" id="person-parent" ref={personDivRef}>
                  {personDivChildren}
                  <div className="w-full flex items-center justify-center">
                    <Button
                      text={"+"}
                      onClick={() => {
                        setPersonDivChildren([person, ...personDivChildren])
                      }}
                    />
                  </div>
                </div>
              </div>
            </div>
          </div>
          <Button
            id="submit"
            text={step === 1 ? "Weiter" : "Abschicken"}
            onClick={() => {
              setStep(2);
              console.log(Form.getExport());
            }}
          />
        </Form.Body>
        <Typography
          variant="subtitle1"
          className="absolute right-[1.35rem] bottom-2.5"
        >
          {step}/2
        </Typography>
      </Paper>
    </PageFrame>
  );
}

/**
 *
 */
