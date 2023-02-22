import { Typography } from "@mui/material";
import { Box } from "@mui/system";
import Paper from "../components/atoms/Paper";
import PageFrame from "../components/PageFrame";
import { useForm } from "react-hook-form";
import Input from "../components/atoms/Input";

export default function Signup({}) {
  const { register, handleSubmit, formState: errors } = useForm();
  const handleSignup = (data) => console.log(data);

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
          <div className="grid grid-cols-1 md:grid-cols-2 w-full">
            <div className="w-full">
              <Typography variant="h6">Firmendetails</Typography>
              <div className="pl-3 w-full">
                <div className="mt-2 border-l border-l-dark px-4 py-2 w-full">
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
                </div>
              </div>
            </div>
            <div>

            </div>
          </div>
        </form>
      </Paper>
    </PageFrame>
  );
}
