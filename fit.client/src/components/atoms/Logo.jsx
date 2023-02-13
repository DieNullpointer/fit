import PropTypes from "prop-types";

export default function Logo({ rounded, outline, size, className}) {
  return (
    <img
      src={"../../logo.png"}
      alt=""
      className={`${className} ${outline ? "border border-gray-300" : ""} ${
        rounded && "rounded-sm"
      } ${
        !size || size === "sm"
          ? "h-12"
          : size === "xs"
          ? "h-6"
          : size === "md"
          ? "h-16"
          : size === "lg"
          ? "h-28"
          : ""
      }`}
    />
  );
}

Logo.propTypes = {
  rounded: PropTypes.bool,
  outline: PropTypes.bool,
  size: PropTypes.oneOf(["xs", "sm", "md", "lg"]),
  white: PropTypes.bool,
};

Logo.defaultProps = {
  rounded: false,
  outline: false,
  size: "sm",
  white: false,
};
