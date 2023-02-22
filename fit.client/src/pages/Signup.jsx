import { Typography } from "@mui/material";
import { Box } from "@mui/system";
import Paper from "../components/atoms/Paper";
import PageFrame from "../components/PageFrame";
import { useForm } from "react-hook-form";
import Input from "../components/atoms/Input";
import AutoComplete from "../components/atoms/AutoComplete";
import Checkbox from "../components/atoms/Checkbox";
import { useState } from "react";

export default function Signup() {
  const { register, handleSubmit, formState: errors } = useForm();
  const handleSignup = (data) => console.log(data);

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
        <form onSubmit={handleSubmit(handleSignup)} className="py-4 px-8">
          <div className="w-full">
            <Typography variant="h6">Firmendetails</Typography>
            <div className="pl-3 w-full">
              <div className="mt-2 border-l border-l-dark px-4 py-2 w-full">
                <div className="grid grid-cols-1 md:grid-cols-2">
                  <div className="md:pr-3">
                    <Input
                      id="in-comp-name"
                      label="Firmenname"
                      {...register("name")}
                      purpose={"text"}
                      required
                      block
                      full
                    />
                    <Input
                      id="in-comp-address"
                      label="Firmenadresse"
                      {...register("address")}
                      purpose={"text"}
                      required
                      block
                      full
                    />
                    <div className="grid grid-cols-3">
                      <Input
                        id="in-comp-plz"
                        label="PLZ"
                        {...register("zipcode")}
                        purpose="text"
                        required
                      />
                      <div className="col-span-2 ml-4">
                        <AutoComplete
                          id="in-country"
                          label="Land"
                          options={["Österreich", "Deutschland", "Schweiz"]}
                          required
                          full
                        />
                      </div>
                    </div>
                  </div>
                  <div className="md:pl-3">
                    <Input
                      id="in-comp-payaddress"
                      label="Rechnungsadresse"
                      {...register("payaddress")}
                      purpose={"text"}
                      required
                      disabled={payDisabled}
                      block
                      full
                    />
                    <div className="-mt-6">
                      <Checkbox
                        label="Gleich wie Firmenadresse"
                        onChange={(e) => setPayDisabled(e.target.checked)}
                        defaultChecked={payDisabled}
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </form>
      </Paper>
    </PageFrame>
  );
}
