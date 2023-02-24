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
      <Paper elevation={3} className="mt-12">
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
                    <div className="grid grid-cols-3">
                      <Input
                        id="in-comp-zip"
                        label="PLZ"
                        purpose="text"
                        required
                        {...Form.Child("input", "zip")}
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
                  </Form.Section>
                  <div className="md:pl-3">
                    <Input
                      id="in-comp-payaddress"
                      label="Rechnungsadresse"
                      purpose={"text"}
                      required
                      disabled={payDisabled}
                      block
                      full
                      {...Form.Child("input", "payaddress")}
                    />
                    <div className="-mt-6">
                      <Checkbox
                        label="Gleich wie Firmenadresse"
                        onChange={(e) => setPayDisabled(e.target.checked)}
                        //defaultChecked={payDisabled}
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <Button id="submit" text="Absenden" {...Form.Submit()} />
        </Form.Body>
      </Paper>
    </PageFrame>
  );
}
