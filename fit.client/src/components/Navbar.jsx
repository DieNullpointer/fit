import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import Menu from "@mui/material/Menu";
import Container from "@mui/material/Container";
import Avatar from "@mui/material/Avatar";
import Button from "@mui/material/Button";
import Tooltip from "@mui/material/Tooltip";
import MenuItem from "@mui/material/MenuItem";
import Logo from "./atoms/Logo";
import Style from "../styleConstants";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";

export default function Navbar({ pages, profileSettings }) {
  const [anchorElNav, setAnchorElNav] = React.useState(null);
  const [anchorElUser, setAnchorElUser] = React.useState(null);

  const handleOpenNavMenu = (event) => {
    setAnchorElNav(event.currentTarget);
  };
  const handleOpenUserMenu = (event) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  const navigate = useNavigate();

  return (
    <AppBar
      position="static"
      color="primary"
      sx={{ bgcolor: Style.colors.white, color: Style.colors.dark }}
    >
      <Container maxWidth="xl">
        <Toolbar disableGutters sx={{
          display: "flex",
          justifyItems: "center",
          justifyContent: "center",
          position: "relative"
        }}>
          <Logo className="mr-2 absolute top-2 left-2" />
          <Link to="/">
            <Typography
              variant="h5"
              noWrap
              sx={{
                mr: 2,
                display: "flex",
                flexGrow: 1,
                fontWeight: 700,
                letterSpacing: ".3rem",
                color: Style.colors.dark,
                textDecoration: "none",
              }}
            >
              Firmen Informationstag
            </Typography>
          </Link>
        </Toolbar>
      </Container>
    </AppBar>
  );
}
