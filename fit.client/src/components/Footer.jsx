import Logo from "./atoms/Logo";
//import Label from "./Label";
import { Link } from "react-router-dom";

export default function Footer({ oldschool }) {
  return oldschool ? (
    <div className="bg-dark p-3 items-center w-full">
      <div className="w-full flex justify-around text-white">
        <div>&copy; HTL Spengergasse</div>
        <div>
          <Link className="hover:underline mr-2" to="/datenschutz">
            Datenschutz
          </Link>

          <Link className="hover:underline" to="/impressum">
            Impressum
          </Link>
        </div>
      </div>
    </div>
  ) : (
    <div className="bg-primary p-2 items-center w-full py-6">
      <div className="w-full grid grid-cols-1 sm:grid-cols-3">
        <div className="flex justify-center items-center">
          <span className="text-xl text-dimmed font-okineLight pr-1">
            Im Auftrag der
          </span>
          <a
            rel="noreferrer"
            className="text-xl text-dimmed hover:underline font-okine"
            target="_blank"
            href="https://www.spengergasse.at"
          >
            Spengergasse
          </a>
        </div>
        <div className="flex justify-center items-center">
          <Logo size="md" white />
        </div>
        <div className="flex justify-center items-center">
          <Link
            className="text-xl text-dimmed hover:underline font-okine"
            to="/Impressum"
          >
            Impressum
          </Link>
        </div>
      </div>
      <p className="text-dimmed font-okineLight text-center text-sm mt-2">
        &copy; Nullpointer, 2023
      </p>
    </div>
  );
}
