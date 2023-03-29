import React, { useEffect, useState } from "react";
import Button from "../components/atoms/Button";
import { Typography } from "@mui/material";
import { Box } from "@mui/system";
import PageFrame from "../components/PageFrame";
import { useNavigate } from "react-router-dom";

export default function NotFound()
{
    const navigate = useNavigate();
    return(
    <PageFrame>
      <Box sx={{ color: "dark", textAlign: "center" }}>
        <Typography variant="h4">404 NotFound</Typography>
        <Typography variant="subtitle1" gutterBottom>
          Hier gehts weiter
        </Typography>
        <Button text={"Homepage"} onClick={() => navigate('/')} ></Button>
      </Box>
    </PageFrame>
    );
}