import PropTypes from "prop-types";

export default function Button({
  onClick,
  id,
  rounded,
  size,
  color,
  text,
  focused,
  nomargin,
  sharp,
  className,
  full,
  centerText,
  minimalPadding,
  children,
  loading,
  disabled,
  transition
}) {
  let viewBoxSize =
    size === "lg" ? 20 : size === "md" ? 11 : size === "sm" ? 10 : 25;

  return (
    <button
      onClick={onClick}
      color="button"
      id={id}
      disabled={disabled}
      className={`${!transition || transition === "default" ? "transition ease-in-out duration-150" : transition === "none" ? "" : ""} font-okine inline-flex items-center focus:outline-none focus:ring-2 border-0 font-medium shadow-sm bg-red ${
        nomargin ? "" : "m-3"
      } ${rounded ? "rounded-full" : sharp ? "rounded-sm" : "rounded-md"} ${
        !size || size === "sm"
          ? "text-xs py-1.5 px-2"
          : size === "md"
          ? "text-sm py-2 px-3"
          : size === "md2"
          ? "text-base py-2.5 px-3"
          : size === "lg"
          ? "text-lg py-3 px-4"
          : size === "xl" && "text-2xl py-6 px-8"
      } ${minimalPadding && "px-0 py-0 !p-2"} ${
        !disabled
          ? color === "primary"
            ? `text-white ${
                focused ? "bg-primaryHover" : "bg-primary"
              } hover:bg-primaryHover focus:ring-primaryHover/50 focus:bg-primaryHover`
            : color === "dark"
            ? `text-white ${
                focused ? "bg-darkBold" : "bg-dark"
              } hover:bg-darkHover focus:ring-slate-600/50`
            : color === "dark-primary"
            ? `text-white ${
                focused ? "bg-darkBold" : "bg-dark"
              } hover:bg-primaryHover focus:ring-primaryHover/50 focus:bg-primaryHover`
            : color === "navbar"
            ? `text-white ${
                focused ? "bg-darkPrimary" : "bg-primary"
              } hover:bg-primaryHover focus:ring-primaryHover/50 focus:bg-primaryHover`
            : color === "dark-red"
            ? `text-white ${
                focused ? "bg-darkBold" : "bg-dark"
              } hover:bg-red-600 focus:ring-red-600/50 focus:bg-red-600`
            : color === "dark-blue"
            ? `text-white ${
                focused ? "bg-darkBold" : "bg-dark"
              } hover:bg-blue-600 focus:ring-blue-600/50 focus:bg-blue-600`
            : color === "white"
            ? `text-dark ${
                focused ? "bg-gray-300" : "bg-gray-100"
              } hover:bg-gray-200 focus:ring-gray-300/50 focus:bg-gray-300`
            : color === "fullwhite" &&
              `text-dark ${
                focused ? "bg-gray-300" : "bg-gray-white"
              } hover:bg-gray-200 focus:ring-gray-300/50 focus:bg-gray-300`
          : `text-gray-500 ${focused ? "bg-gray-500" : "bg-gray-300"}`
      } ${full && "w-full h-full"} ${
        centerText && "text-center justify-center"
      } ${className}`}
    >
      {!loading ? (
        text || children
      ) : (
        <svg
          fill="none"
          className="animate-spin m-1"
          height={viewBoxSize}
          viewBox={`0 0 20 20`}
          width="20"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M10 3.5C6.41015 3.5 3.5 6.41015 3.5 10C3.5 10.4142 3.16421 10.75 2.75 10.75C2.33579 10.75 2 10.4142 2 10C2 5.58172 5.58172 2 10 2C14.4183 2 18 5.58172 18 10C18 14.4183 14.4183 18 10 18C9.58579 18 9.25 17.6642 9.25 17.25C9.25 16.8358 9.58579 16.5 10 16.5C13.5899 16.5 16.5 13.5899 16.5 10C16.5 6.41015 13.5899 3.5 10 3.5Z"
            fill="white"
          />
        </svg>
      )}
    </button>
  );
}

export const btnColors = [
  "primary",
  "dark",
  "dark-primary",
  "navbar",
  "white",
  "dark-red",
  "dark-blue",
  "fullwhite"
];

Button.propTypes = {
  color: PropTypes.oneOf(btnColors).isRequired,
  size: PropTypes.oneOf(["sm", "md", "md2", "lg", "xl"]),
  text: PropTypes.string,
  rounded: PropTypes.bool,
  sharp: PropTypes.bool,
  onClick: PropTypes.func,
  focused: PropTypes.bool,
  nomargin: PropTypes.bool,
  centerText: PropTypes.bool,
};

Button.defaultProps = {
  size: "sm",
  rounded: false,
  sharp: false,
  focused: false,
  nomargin: false,
};
