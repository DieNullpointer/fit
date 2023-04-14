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
import { useNavigate, } from "react-router-dom";
import { motion as m } from "framer-motion";

export default function Signup() {
  const [payDisabled, setPayDisabled] = useState(false);
  const [events, setEvents] = useState([]);
  const [packages, setPackages] = useState([]);

  const [page, setPage] = useState(1)

  const navigate = useNavigate();

  useEffect(() => {
    init();
  }, [page]);

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
      
      <m.div
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 0.85 }}
      >
        <Paper elevation={3} className="mt-12 mb-24 relative">
          <Form.Body
          
            className="py-4 px-8"
            id="form-1"
          >
            <div className={`transition ease-in-out duration-700`}>
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
                        {...Form.Child("autocomplete", "event", (e, newval) => {
                          const text = newval.text;
                          let correctGuid = 1;
                          events.forEach((ev) => {
                            if (ev.text === text)
                              return (correctGuid = ev.guid);
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
                          {...Form.Child("autocomplete", "package", (e, newval) => {
                            const text = newval.text;
                            let correctGuid = 1;
                            packages.forEach((p) => {
                              if (p.text === text)
                                return (correctGuid = p.guid);
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

            <Button
              id="submit"
              text="Weiter"
              className="mb-3"
              onClick={() => {
                sessionStorage.setItem("signup1", JSON.stringify(Form.getExport()));
                console.log(Form.reset());
                navigate("/signup/continue");
              }}
              
            />
          </Form.Body>
          <Typography
            variant="subtitle1"
            className="absolute right-[1.35rem] bottom-2.5"
          >
            1/2
          </Typography>
        </Paper>
      </m.div>
      
    </PageFrame>
  );
}
