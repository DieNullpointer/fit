import Footer from "./Footer";
import Navbar from "./Navbar";

export default function PageFrame({ children, active, margin, className }) {
  return (
    <div>
      <div className={`min-h-screen ${className}`}>
        <Navbar
          pages={[
            {name: "home", href: "/", active: active === "home"},
            { name: "sign-up", href: "/signup", active: active === "sign-up" },
            { name: "about", href: "/about", active: active === "about" },
          ]}
          profileSettings
        />
        <div className={`${margin ? "mt-16" : ""} mx-10`}>
          {children}
        </div>
      </div>
      <Footer oldschool />
    </div>
  );
}
