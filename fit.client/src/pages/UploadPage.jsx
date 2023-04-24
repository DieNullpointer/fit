import { useEffect } from "react";
import APIConstants from "../apiConstants";
import PageFrame from "../components/PageFrame";
import { useParams } from "react-router-dom";
import { Typography } from "@mui/material";

export default function UploadPage() {
  const { company } = useParams();

  useEffect(() => {
    init();
  }, [company]);

  async function init() {
    if (company) {
      APIConstants.getCompany(company);
    } else {

    }
  }

  return <PageFrame margin className="bg-primary">
    <Typography variant="h4" color="white">Fast geschafft!</Typography>
    <Typography variant="subtitle1" gutterBottom color="white">Laden Sie als letztes noch nötige Dateien hoch.</Typography>
  </PageFrame>;
}