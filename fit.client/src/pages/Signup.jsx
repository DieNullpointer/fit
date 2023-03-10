import { Typography } from "@mui/material";
import { Box } from "@mui/system";
import Paper from "../components/atoms/Paper";
import PageFrame from "../components/PageFrame";
import Input from "../components/atoms/Input";
import AutoComplete from "../components/atoms/AutoComplete";
import Checkbox from "../components/atoms/Checkbox";
import { useState } from "react";
import Button from "../components/atoms/Button";
import Form from "../components/form/Form";
import APIConstants from "../apiConstants";

export default function Signup() {
  const [payDisabled, setPayDisabled] = useState(false);

  return (
    <PageFrame active={"sign-up"} margin className="bg-primary">
      <Box sx={{ color: "white", textAlign: "center" }}>
        <Typography variant="h4">Anmeldung für den FIT</Typography>
        <Typography variant="subtitle1" gutterBottom>
          Dieses Formular dient zur Anmeldung zukünfitger FITs
        </Typography>
      </Box>
      <Paper elevation={3} className="mt-12 relative">
        <Typography
          variant="subtitle1"
          className="absolute right-[1.35rem] top-2.5"
        >
          1/2
        </Typography>
        <Form.Body className="py-4 px-8">
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
            <Typography variant="h6">Ansprechpartner</Typography>
            <div className="pl-3 w-full">
              <div className="mt-2 border-l border-l-dark px-4 py-2 w-full">
                <Form.Section className="md:pr-3">
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
              </div>
            </div>
          </div>
          <div className="w-full">
            <Typography variant="h6">Paketauswahl</Typography>
            <div className="pl-3 w-full">
              <div className="mt-2 border-l border-l-dark px-4 py-2 w-full">
                
              </div>
            </div>
          </div>
          <Button
            id="submit"
            text="Absenden"
            {...Form.Submit(APIConstants.COMPANY_URL + "/register")}
          />
        </Form.Body>
      </Paper>
    </PageFrame>
  );
}
