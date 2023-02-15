
import { CssBaseline } from "@mui/material";
import { ThemeProvider } from "@mui/material";
import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
import Home from "./pages/Home";
import NotFound from "./pages/NotFound";
import Admin from "./pages/Admin";
import AddEvent from "./pages/AddEvent";
import Style from "./styleConstants";

export default function App() {
  return (
    <ThemeProvider theme={Style.theme}>
      <CssBaseline />
      <Router>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="*" exact={true} element={<NotFound />} />
          <Route path="/admin" element={<Admin />} />
          <Route path="/event/add" element={<AddEvent />} />
        </Routes>
      </Router>

    </ThemeProvider>
  );
}
