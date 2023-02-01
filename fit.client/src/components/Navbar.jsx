import PropTypes from "prop-types";
import Logo from "./atoms/Logo";
import { Link, useNavigate } from "react-router-dom";
import Image from "./atoms/Image";
import { useState, useEffect } from "react";
import React from "react";
import Button from "./atoms/Button";
//import ApiConstants from "../apiConstants";

export default function Navbar({ hrefHome, options, update, onProfileClick }) {
  const navigate = useNavigate();
  return (
    <></>
  );
}

Navbar.propTypes = {
  color: PropTypes.oneOf(["primary", "dark", "white"]),
  hrefHome: PropTypes.string,
  options: PropTypes.array,
};

Navbar.defaultProps = {
  color: "primary",
};
