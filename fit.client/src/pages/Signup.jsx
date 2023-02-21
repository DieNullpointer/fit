import { Typography } from "@mui/material";
import { Box } from "@mui/system";
import Paper from "../components/atoms/Paper";
import PageFrame from "../components/PageFrame";
import { useForm } from "react-hook-form";

export default function Signup({}) {
  const { signup, handleSubmit, formState: errors } = useForm();
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
        <div className="flex justify-center items-center">
          <form onSubmit={handleSubmit(handleSignup)}>
            abc
          </form>
        </div>
      </Paper>
    </PageFrame>
  );
}
