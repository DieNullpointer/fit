import Footer from "./Footer";
import Navbar from "./Navbar";

export default function PageFrame({
  children, active
}) {


  return (
    <div>
      <div className="min-h-screen">
      <Navbar
          pages={[{ name: "sign-up", active: active === "sign-up" }, { name: "about", active: active === "about" }]}
          profileSettings
        />
        {children}
      </div>
      <Footer oldschool />
    </div>
  );
}
