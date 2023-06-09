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

function Body({ className, children, active, id, definedUseEffect }) {
  React.useEffect(() => {
    if (active) {
      for (let i = 0; i < idArray.length; i++) {
        stateArray[i][1](refArray[i].current);
      }
      if (definedUseEffect) definedUseEffect();
    }
  });

  return <form className={className}>{children}</form>;
}

// check for formchild of active body
function Section({ children, className, array, id }) {
  return (
    <div id={id} className={className}>
      {children}
    </div>
  );
}

/**
 * @param {"input" | "button" | "checkbox" | "autocomplete"} type Type of FormChild
 */
function Child(type, name, onChangeOverride, valueOverride = false) {
  let count = 0;
  if (idArray.includes(name)) {
    count = idArray.indexOf(name);
  }

  const onChange = (e, newval) => {
    if (!onChangeOverride)
      refArray[count].current =
        type === "autocomplete" ? newval.text : e.target.value;
    else refArray[count].current = onChangeOverride(e, newval);
    console.log("Ref in onchange: " + refArray[count].current);
  };

  if (!valueOverride) {
    if (type === "input")
      return {
        as: Get(type, name),
        onChange,
        defaultValue: refArray[count].current || "",
      };
    else if (type === "autocomplete")
      return {
        as: Get(type, name),
        onChange,
        defaultValue: refArray[count].current || null,
      };
  } else return { as: Get(type, name), onChange };
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
    console.log([idArray[i], refArray[i].current, stateArray[i][0]]);
    Object.defineProperty(exportObj, idArray[i], {
      value: refArray[i].current,
      writable: true,
      enumerable: true,
    });
  }
  console.log(exportObj);
  return exportObj;
}

function addManual(name, gvalue) {
  let fidx = 0;
  idArray.find((value, idx) => {
    if (value === name) {
      fidx = idx;
      return true;
    }
  });
  refArray[fidx].current = gvalue;
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
export default {
  Body,
  Section,
  Child,
  getExport,
  reset,
  Reload,
  addManual,
};
