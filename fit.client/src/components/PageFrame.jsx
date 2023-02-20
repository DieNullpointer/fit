import Footer from "./Footer";
import Navbar from "./Navbar";

export default function PageFrame({ children, active }) {
  return (
    <div>
      <div className="min-h-screen">
        <Navbar
          pages={[
            {name: "home", href: "/", active: active === "home"},
            { name: "sign-up", href: "/signup", active: active === "sign-up" },
            { name: "about", href: "/about", active: active === "about" },
          ]}
          profileSettings
        />
        {children}
      </div>
      <Footer oldschool />
    </div>
  );
}
