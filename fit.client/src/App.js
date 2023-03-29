
import { CssBaseline } from "@mui/material";
import { ThemeProvider } from "@mui/material";
import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
import Home from "./pages/Home";
import NotFound from "./pages/NotFound";
import Admin from "./pages/Admin";
import AddEvent from "./pages/AddEvent";
import Style from "./styleConstants";
import AddPackage from "./pages/AddPackage";
import Events from "./pages/Events";
import Packages from "./pages/Packages";
import Signup from "./pages/Signup";

export default function App() {
  return (
    <ThemeProvider theme={Style.theme}>
      <CssBaseline />
      <Router>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/admin" element={<Admin />} />
          <Route path="/event/add" element={<AddEvent />} />
          <Route path="/package/add" element={<AddPackage />} />
          <Route path="/package" element={<Packages />} />
          <Route path="/event" element={<Events />} />
          <Route path="/impressum" />
          <Route path="/datenschutz" />
          <Route path="/signup" element={<Signup />} />
          <Route path="*" exact={true} element={<NotFound />} />
        </Routes>
      </Router>

    </ThemeProvider>
  );
}
