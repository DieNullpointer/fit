import PropTypes from "prop-types";
import { Link } from "react-router-dom";

export default function Image({
  src,
  alt,
  className,
  full,
  vanilla,
  sharp,
  rounded,
  divclass,
  href,
  routerDOM,
  screen,
}) {
  const img = (
    <img
      className={`${className} ${
        sharp ? "rounded-none" : rounded ? "rounded-full" : "rounded-md"
      }`}
      src={src}
      alt={alt}
    />
  );

  const content = href ? (
    routerDOM ? (
      <Link to={href}>{img}</Link>
    ) : (
      <a href={href}>{img}</a>
    )
  ) : (
    img
  );

  return (
    <>
      {!vanilla ? (
        <div
          className={`${
            full
              ? "w-full h-full"
              : screen
              ? "w-screen h-auto"
              : "max-w-lg sm:max-w-lg h-auto"
          } ${divclass}`}
        >
          {content}
        </div>
      ) : divclass ? (
        <div className={divclass}>{content}</div>
      ) : (
        content
      )}
    </>
  );
}

Image.propTypes = {
  src: PropTypes.string.isRequired,
  alt: PropTypes.string,
  full: PropTypes.bool,
  sharp: PropTypes.bool,
  vanilla: PropTypes.bool,
  rounded: PropTypes.bool,
  href: PropTypes.string,
  routerDOM: PropTypes.bool,
  screen: PropTypes.bool,
};

Image.defaultProps = {
  full: false,
  sharp: false,
  vanilla: false,
  rounded: false,
  routerDOM: false,
  screen: false,
};
