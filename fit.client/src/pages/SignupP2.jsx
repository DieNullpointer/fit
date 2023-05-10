import { Typography } from "@mui/material";
import { Box } from "@mui/system";
import Paper from "../components/atoms/Paper";
import PageFrame from "../components/PageFrame";
import { useState, useEffect, useRef } from "react";
import Button from "../components/atoms/Button";
import { useNavigate } from "react-router-dom";
import { motion as m } from "framer-motion";
import { SignupPerson as Person } from "../components/form/SignupPerson";
import APIConstants from "../apiConstants";

export default function Signup() {
  const navigate = useNavigate();
  const url = window.location.pathname.split("/").pop();

  useEffect(() => {
    if (!sessionStorage.getItem("signup1")) navigate("/signup");
  }, [url]);

  const mainPersonId = useRef(0);

  let data = [];
  const updateData = (number, rdata) => {
    var found = false;
    data.forEach((person) => {
      if (person._intern === number) {
        person = rdata;
        found = true;
      }
    });

    if (!found) data.push(rdata);
  };

  const [persons, setPersons] = useState([0, 1, 2]);
  const [error, setError] = useState({});

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
          <div className="py-4 px-8" id="form-2">
            <div className={`transition ease-in-out duration-700`}>
              <div className="w-full">
                <Typography variant="h6">Ansprechpartner</Typography>
                <div className="pl-3 w-full">
                  <div
                    className="mt-2 border-l border-l-dark px-4 py-2 w-full"
                    id="person-parent"
                  >
                    {persons.map((value, idx) => (
                      <Person
                        number={value}
                        onChange={(number, rdata, e) => {
                          //recieved data = rdata from person component
                          updateData(number, rdata);
                        }}
                        
                      />
                    ))}
                    {/**
                       * <div className="w-full flex items-center justify-center">
                      <Button
                        text={"+"}
                        onClick={() => {
                          setPersons([persons.length, ...persons])
                          console.log(persons.length);
                        }}
                      />
                    </div>
                       */}
                  </div>
                </div>
              </div>
            </div>
            <div className="flex flex-row justify-between mb-3">
              {/**
                   * <Button
                  id="back"
                  text="Zurück"
                  onClick={() => {
                    Form.reset();
                    navigate("/signup");
                  }}
                />
                   */}
              
              <Button
                id="submit"
                text="Weiter"
                onClick={async () => {
                  data.forEach((p) => {
                    delete p._intern;
                  });
                  sessionStorage.setItem("signup2", JSON.stringify(data));
                  let exportObj = JSON.parse(sessionStorage.getItem("signup1"));
                  Object.defineProperty(exportObj, "partners", {
                    value: data,
                    writable: true,
                    enumerable: true,
                  });
                  console.log(exportObj);
                  let response = await APIConstants.registerCompany(exportObj);

                  if (response != true) {
                    let errors = [];
                    console.log("errors:");
                    Object.entries(response).forEach(([key, value]) => {
                      console.log(value[0]);
                      errors.push(value[0]);
                    });

                    setError({ msg: errors.join("; ") });
                  }
                }}
              />
              {error.msg && (
                <div className="m-2">
                  <Typography color="crimson" variant="subtitle1" sx={{ lineHeight: "20px" }}>
                  {error.msg}
                </Typography>
                </div>
              )}
            </div>
            <Typography
              variant="subtitle1"
              className="absolute right-[1.35rem] bottom-2.5"
            >
              (Dev Note: temporarily only three options) 2/2
            </Typography>
          </div>
        </Paper>
      </m.div>
    </PageFrame>
  );
}
