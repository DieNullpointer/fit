import React, { useEffect, useState } from "react";
import { Typography } from "@mui/material";
import { Box } from "@mui/system";
import { Link, useNavigate } from "react-router-dom";
import PageFrame from "../components/PageFrame";
import { motion as m } from "framer-motion";

export default function Signup() {
  const [seconds, setSeconds] = useState(15);
  const navigate = useNavigate();

  useEffect(() => {
    if(!(seconds < 0)) {
        setTimeout(() => {
            setSeconds(seconds - 1);
        }, 1000);
    } else navigate(`/companypage/${sessionStorage.getItem("companyGuid")}`);
  }, [seconds]);

  return (
    <PageFrame active={"sign-up"} margin className="bg-primary">
      <m.div
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 0.85 }}
      >
        <Box sx={{ color: "white", textAlign: "center" }}>
          <Typography variant="h4" gutterBottom>Danke für die Anmeldung!</Typography>
          <Typography variant="subtitle1">
            Sie wurden erfolgreich angemeldet. Überprüfen Sie Ihr E-Mail
            Postfach.
          </Typography>
          <Typography variant="subtitle1" gutterBottom>
            Sie werden in Kürze zum Firmenportal weitergeleitet... {seconds}
          </Typography>
          <div className="flex justify-center items-center">
          <img src="../alternative_spg_logo.png" alt="SPG Logo" width="320" />
          </div>
          <Typography marginTop={"40px"} gutterBottom>
            Redirect funktioniert nicht? Versuchen Sie diesen <Link className="text-blue-300" to={`/companypage/${sessionStorage.getItem("companyGuid")}`}>Link</Link>.
          </Typography>
        </Box>
      </m.div>
    </PageFrame>
  );
}
