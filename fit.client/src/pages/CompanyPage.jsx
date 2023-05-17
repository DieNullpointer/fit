import { useEffect, useState } from "react";
import APIConstants from "../apiConstants";
import PageFrame from "../components/PageFrame";
import { useParams } from "react-router-dom";
import { Typography } from "@mui/material";
import { motion as m } from "framer-motion";

export default function UploadPage() {
  const { company } = useParams();
  const [comp, setComp] = useState();

  useEffect(() => {
    init();
  }, [company]);

  async function init() {
    if (!sessionStorage.getItem("companyGuid"))
      sessionStorage.setItem("companyGuid", company);

    if (company) {
      let fetchedResults = await APIConstants.getCompany(company);
      console.log(fetchedResults);
       setComp(fetchedResults);
      sessionStorage.setItem("company", JSON.stringify(fetchedResults));
    }

  }

  return (
    <PageFrame margin className="bg-primary">
      <m.div
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 0.85 }}
      >
        <Typography variant="h4" color="white">
          Firmenportal
        </Typography>
        <Typography variant="subtitle1" gutterBottom color="white">
          Ihre persönliche Seite für Organisatorisches
        </Typography>
        <Typography variant="subtitle1" color="white" marginTop="15px"> 
          <b>Firma:</b> {comp?.name}
        </Typography>
        {comp?.contactPartners && (
          <Typography variant="subtitle1" gutterBottom color="white" marginTop="15px">
             <b>Ansprechpartner:</b><br />{comp?.contactPartners
              .map(
                p =>
                  <>{p.title} <b>{p.firstname} {p.lastname}</b> (<i>{p.email}</i>; <i>{p.telNr}</i>)</>
              )}
          </Typography>
        )}
      </m.div>
    </PageFrame>
  );
}
