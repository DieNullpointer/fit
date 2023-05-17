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
      var fetchCompany = await APIConstants.getCompany(company);
      console.log(fetchCompany);
      setComp(fetchCompany);
      console.log(comp);
      sessionStorage.setItem("company", JSON.stringify(fetchCompany));
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
        {comp.contactPartners && (
          <Typography variant="subtitle1" gutterBottom color="white">
            {comp.contactPartners
              .map(
                (idx, p) =>
                  `${p.title} ${p.firstname} ${p.lastname} (${p.email} ${p.telNr})`
              )
              .join(" ")}
          </Typography>
        )}
      </m.div>
    </PageFrame>
  );
}
