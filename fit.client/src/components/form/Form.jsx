import React from "react";

let stateArray = [];
let refArray = [];
let idArray = [];
let sections = [];

function Body({ className, children }) {
  React.useEffect(() => {
    for (let i = 0; i < idArray.length; i++) {
      stateArray[i][1](refArray[i].current);
    }
  });

  function walkAllChildren(root, callback) {
    function walk(e, parents) {
      callback(e, parents);
      const newParents = [...parents, e];
      React.Children.toArray(e.props?.children).forEach((c) => {
        walk(c, newParents);
      });
    }
    walk(root, []);
  }

  walkAllChildren(<>{children}</>, (e, parents) => {
    if (e.type?.name === "Section") sections.push(e);
  });

  if (!sections.length) {
    console.error(
      "[FIT-Manager Component] Form.Body must have at least one Form.Section element!"
    );
  }

  return <form className={className}>{children}</form>;
}

function Section({ children, className }) {
  return <div className={className}>{children}</div>;
}

/**
 * @param {"input" | "button" | "checkbox" | "autocomplete"} type Type of FormChild
 */
function Child(type, name) {
  let count = 0;
  if (idArray.includes(name)) {
    count = idArray.indexOf(name);
  }
  const onChange = (e) => {
    refArray[count].current = (type === "autocomplete") ? e.target.innerText : e.target.value;
  };
  return { as: Get(type, name), onChange };
}

function Submit() {
  // eslint-disable-next-line
  const [submit, setSubmit] = React.useState(false);

  return {
    onClick: () => {
      setSubmit(true);
      getExport();
    },
  };
}

function Get(type, name) {
  const idString = name;
  const [state, setState] = React.useState(type === "input" ? "" : false);
  const ref = React.useRef(state);
  if (!idArray.includes(idString)) {
    idArray.push(idString);
    stateArray.push([state, setState]);
    refArray.push(ref);
    console.log("Added new Input with id: " + idString);
  }
  return { id: idArray.length - 1 };
}

function getExport() {
  let exportObj = {};
  for (let i = 0; i < idArray.length; i++) {
    Object.defineProperty(exportObj, idArray[i], {
      value: refArray[i].current,
      writable: true,
    });
  }
  console.log(exportObj);
}

// eslint-disable-next-line
export default { Body, Section, Child, Submit };
