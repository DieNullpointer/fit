import Footer from "./Footer";
import Navbar from "./Navbar";

export default function PageFrame({ children, active, margin, className, noFullScreen, innerClassName }) {
  return (
    <div className={className}>
      <div className={`${!noFullScreen ? "min-h-screen" : ""} h-full mb-0`}>
        <Navbar
          pages={[
            {name: "home", href: "/", active: active === "home"},
            { name: "sign-up", href: "/signup?p=1", active: active === "sign-up" },
            { name: "about", href: "/about", active: active === "about" },
          ]}
          profileSettings
        />
        <div className={`${margin ? "mt-16" : ""} ${innerClassName} mx-10`}>
          {children}
        </div>
      </div>
      <Footer oldschool />
    </div>
  );
}
