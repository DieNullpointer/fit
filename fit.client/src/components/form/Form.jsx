import React from "react";
import axios from "axios";

let stateArray = [];
let refArray = [];
let idArray = [];
let sections = [];

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

function Body({ className, children, active, id }) {
  React.useEffect(() => {
    if (active) {
      for (let i = 0; i < idArray.length; i++) {
        stateArray[i][1](refArray[i].current);
      }
    }
  });

  return <form className={className}>{children}</form>;
}

// check for formchild of active body
function Section({ children, className, array }) {
  return <div className={className}>{children}</div>;
}

/**
 * @param {"input" | "button" | "checkbox" | "autocomplete"} type Type of FormChild
 */
function Child(type, name, onChangeOverride) {
  let count = 0;
  if (idArray.includes(name)) {
    count = idArray.indexOf(name);
  }

  const onChange = (e, newval) => {
    if (!onChangeOverride)
      refArray[count].current =
        type === "autocomplete" ? newval.text : e.target.value;
    else refArray[count].current = onChangeOverride(e, newval);
  };

  return { as: Get(type, name), onChange };
}

function Submit(route) {
  // eslint-disable-next-line
  const [submit, setSubmit] = React.useState(false);

  return {
    onClick: async () => {
      var exportObj = getExport();
      setSubmit(true);
      console.log(exportObj);
      await axios
        .post(route, exportObj)
        .then((response) => {
          console.log(response);
        })
        .catch((e) => {
          console.log(e);
        });
    },
  };
}

function Get(type, name) {
  const idString = name;
  const [state, setState] = React.useState(type === "input" ? "" : false);
  const ref = React.useRef(state);
  if (!idArray.includes(idString) ) {
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
      enumerable: true,
    });
  }
  return exportObj;
}

function reset() {
  let exportObj = getExport();
  stateArray = [];
  refArray = [];
  idArray = [];
  sections = [];
  return exportObj;
}

function Reload() {
  const [reload, setReload] = React.useState(false);
  setReload(false);
  reset();
  return <></>;
}

// eslint-disable-next-line
export default { Body, Section, Child, Submit, getExport, reset, Reload };
