import { useEffect, useState } from "react";
import APIConstants from "../apiConstants";
import PageFrame from "../components/PageFrame";
import { useParams } from "react-router-dom";
import { Typography } from "@mui/material";
import Paper from "../components/atoms/Paper";
import { motion as m } from "framer-motion";
import RichTextEditor from "../components/RichTextEditor";
import Button from "../components/atoms/Button";
import SmallUpload from "../components/atoms/SmallUpload";
import axios from "axios";

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
          <Typography marginTop="15px" color="white" variant="subtitle1">
            <span style={{ color: comp?.hasPaid ? "lime" : "yellow" }}>
              <b>
                &bull; <span style={{ color: "white" }}>Status:</span>
              </b>{" "}
              {comp?.hasPaid ? "bezahlt" : "nicht bezahlt"}
            </span>
          </Typography>
        </div>

        <Paper elevation={3} className="mt-12 mb-10 relative">
          <div className="py-4 px-8">
            <Typography variant="h5" gutterBottom>
              Pakete
            </Typography>
            <div className="pl-3">
              <Typography variant="subtitle1">
                <b>Paket as dem Vorjahr</b>: ?
              </Typography>
              <Typography variant="subtitle1" marginTop="10px">
                <b>Diesjähriges Paket</b>:{" "}
                {comp?.package ? comp.package.name : "nicht ausgewählt"}
              </Typography>
            </div>
          </div>
        </Paper>

        <Paper elevation={3} className="mt-12 mb-24 relative">
          <div className="py-4 px-8">
            <Typography variant="h5" gutterBottom>
              Uploads
            </Typography>
            <div className="pl-3">
              <Typography variant="subtitle1">
                <b>Firmenlogo Upload</b>
              </Typography>
              <SmallUpload
                label="Logo Auswählen"
                helpText="Erlaubte Endungen: JPG; PNG; WEBP"
                id="logoupload"
                action={`${axios.defaults.baseURL}${APIConstants.COMPANY_URL}/addlogo/${comp?.guid}`}
              />
              {comp?.package?.name.includes("Inserat") && (
                <div>
                  <Typography variant="subtitle1" sx={{ marginTop: "23px" }}>
                    <b>Inserat Upload</b>
                  </Typography>
                  <SmallUpload
                    label="Inserat Hochladen"
                    helpText="Erlaubte Endungen: PDF"
                    id="inseratupload"
                    action={`${axios.defaults.baseURL}${APIConstants.COMPANY_URL}/addinserat/${comp?.guid}`}
                  />
                </div>
              )}
              <Typography variant="subtitle1" sx={{ marginTop: "23px" }}>
                <b>Dokumentupload (z.B. Infomaterial)</b>
              </Typography>
              <SmallUpload
                label="Freier Fileupload"
                helpText="Erlaubte Endungen: PDF"
                id="documentupload"
                multiple
                action={`${axios.defaults.baseURL}${APIConstants.COMPANY_URL}/addmultiple/${comp?.guid}`}
                backendName="files"
              />
              <Typography variant="subtitle1" sx={{ marginTop: "23px" }}>
                <b>Selbstdarstellung</b>
              </Typography>
              <div id="text-editor">
                <RichTextEditor placeholder="Stellen Sie Ihre Firma vor" startValue={comp?.description || ""} />
              </div>
              <div className="flex justify-end">
                <Button
                  style={{
                    maxWidth: "200px",
                    maxHeight: "30px",
                    minWidth: "30px",
                    minHeight: "30px",
                  }}
                  text={"Text Speichern"}
                  onClick={() =>
                    axios.post(`${APIConstants.COMPANY_URL}/adddescription`, {
                      guid: comp?.guid,
                      description: sessionStorage.getItem("editorHtml"),
                    })
                  }
                />
              </div>
            </div>
          </div>
        </Paper>
      </m.div>
    </PageFrame>
  );
}
