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
import { useNavigate, useParams, useSearchParams } from "react-router-dom";
import { motion as m } from "framer-motion";

export default function Signup() {
  const navigate = useNavigate();

  useEffect(() => {
    if (!sessionStorage.getItem("signup1")) navigate("/signup");
  });

  let personDivChildrenLength = 0;

  const person = (
    <Paper
      elevation={3}
      id="person-paper"
      key={"paper-key-person-" + personDivChildrenLength++}
    >
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

  const [personDivChildren, setPersonDivChildren] = useState([person]);
  const personDivRef = useRef();

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
          <Form.Body className="py-4 px-8" id="form-2">
            <div className={`transition ease-in-out duration-700`}>
              <div className="w-full">
                <Typography variant="h6">Ansprechpartner</Typography>
                <div className="pl-3 w-full">
                  <div
                    className="mt-2 border-l border-l-dark px-4 py-2 w-full"
                    id="person-parent"
                    ref={personDivRef}
                  >
                    {personDivChildren}
                    <div className="w-full flex items-center justify-center">
                      <Button
                        text={"+"}
                        onClick={() => {
                          setPersonDivChildren([person, ...personDivChildren]);
                        }}
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div className="flex flex-row justify-between mb-3">
            <Button id="back" text="Zurück" onClick={() => {
              Form.reset();
              navigate("/signup")
            }} />
            <Button
              id="submit"
              text="Abschicken"
              onClick={() => {
                sessionStorage.setItem("signup2", JSON.stringify(Form.getExport()));
                console.log(Form.reset())
              }}
            />
            </div>
            <Typography
            variant="subtitle1"
            className="absolute right-[1.35rem] bottom-2.5"
          >
            2/2
          </Typography>
          </Form.Body>
        </Paper>
      </m.div>
    </PageFrame>
  );
}
