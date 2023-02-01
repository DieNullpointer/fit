import PropTypes from "prop-types";

export default function Label({
  text,
  purpose,
  border,
  subtext,
  center,
  color,
  children,
  squeeze,
  className,
  id,
  bgclass,
  textpos,
}) {
  return (
    <div>
      <div
        className={`${
          !textpos || textpos === "left"
            ? "text-left"
            : textpos === "center"
            ? "text-center"
            : textpos === "right" && "text-right"
        } ${border ? "pb-3" : !squeeze ? "p-2" : ""} ${bgclass}`}
      >
        <div
          className={`font-okine ${
            center && "items-center justify-center flex"
          } ${
            !color || color === "dark"
              ? "text-dark"
              : color === "dimmed"
              ? "text-dimmed"
              : color === "primary"
              ? "text-primary"
              : color === "red"
              ? "text-red-600"
              : color === "green"
              ? "text-green-600"
              : color === "slate" && "text-slate-600"
          } ${squeeze ? "mb-0" : ""} ${className}`}
        >
          {!purpose || purpose === "default" ? (
            <p id={id} className={`text-sm font-medium`}>
              {text}
            </p>
          ) : purpose === "h1" ? (
            <h1 id={id} className="text-6xl font-medium">
              {text}
            </h1>
          ) : purpose === "h2" ? (
            <h2 id={id} className="text-4xl font-medium">
              {text}
            </h2>
          ) : purpose === "h3" ? (
            <h3 id={id} className="text-2xl font-medium">
              {text}
            </h3>
          ) : purpose === "h4" ? (
            <h4 id={id} className="text-xl font-medium">
              {text}
            </h4>
          ) : purpose === "h5" ? (
            <h5 id={id} className="text-lg font-medium">
              {text}
            </h5>
          ) : (
            purpose === "h6" && (
              <h6 id={id} className="text-sm font-medium">
                {text}
              </h6>
            )
          )}
        </div>
        <div className={center ? "items-center flex flex-col " : ""}>
          {(subtext || children) && (
            <p
              className={`${
                squeeze
                  ? "mt-0"
                  : purpose === "h1" || purpose === "h2"
                  ? "mt-5"
                  : "mt-2"
              } max-w-4xl text-sm ${
                color === "dimmed" ? "text-dimmed" : "text-gray-500"
              } font-okineLight ${
                (purpose === "h1" || purpose === "h2") && "text-lg"
              }`}
            >
              {children || subtext}
            </p>
          )}
        </div>
      </div>
    </div>
  );
}

Label.propTypes = {
  text: PropTypes.string.isRequired,
  border: PropTypes.bool,
  subtext: PropTypes.string,
  color: PropTypes.oneOf([
    "dimmed",
    "dark",
    "primary",
    "secondary",
    "slate",
    "red",
    "green",
  ]),
  purpose: PropTypes.oneOf(["h1", "h2", "h3", "h4", "h5", "h6", "default"]),
  center: PropTypes.bool,
  squeeze: PropTypes.bool,
  textpos: PropTypes.oneOf(["left", "center", "right"]),
};

Label.defaultProps = {
  color: "dimmed",
  purpose: "default",
  border: false,
  light: false,
  center: false,
  squeeze: false,
  textpos: "left",
};
