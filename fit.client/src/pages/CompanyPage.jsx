import { useEffect, useState } from "react";
import APIConstants from "../apiConstants";
import PageFrame from "../components/PageFrame";
import { useParams } from "react-router-dom";
import { Typography } from "@mui/material";
import Paper from "../components/atoms/Paper";
import { motion as m } from "framer-motion";
import RichTextEditor from "../components/RichTextEditor";
import Button from "../components/atoms/Button";

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
        <div className="pl-3">
          <Typography variant="subtitle1" color="white" marginTop="15px">
            <b>Firma:</b> {comp?.name}
          </Typography>
          {comp?.contactPartners && (
            <Typography
              variant="subtitle1"
              gutterBottom
              color="white"
              marginTop="15px"
            >
              <b>Ansprechpartner:</b>
              <br />
              {comp?.contactPartners.map((p) => (
                <div key={p.lastname}>
                  {p.title}{" "}
                  <b>
                    {p.firstname} {p.lastname}
                  </b>{" "}
                  (<i>{p.email}</i>; <i>{p.telNr}</i>)<br />
                </div>
              ))}
            </Typography>
          )}
        </div>

        <Paper elevation={3} className="mt-12 mb-24 relative">
          <div className="py-4 px-8">
            <Typography variant="h5" gutterBottom>
              Uploads
            </Typography>
            <div className="pl-3">
              <Typography variant="subtitle1">
                <b>Firmenlogo Upload</b>
              </Typography>
              {comp?.package.name.includes("Inserat") && (
                <Typography variant="subtitle1" sx={{marginTop: "23px"}}>
                  <b>Inserat Upload</b>
                </Typography>
              )}
              <Typography variant="subtitle1" sx={{marginTop: "23px"}}>
                <b>Dokumentupload (z.B. Infomaterial)</b>
              </Typography>
              <Typography variant="subtitle1" sx={{marginTop: "23px"}}>
                <b>Selbstdarstellung</b>
              </Typography>
              <div id="text-editor">
                <RichTextEditor placeholder="Stellen Sie Ihre Firma vor" />
              </div>
              <div className="flex justify-end">
              <Button text={"Speichern"} onClick={() => console.log(sessionStorage.getItem("editorHtml"))}/>
            </div>
            </div>
            
          </div>
        </Paper>
      </m.div>
    </PageFrame>
  );
}
