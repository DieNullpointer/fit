import React from "react";

let stateArray = [];
let refArray = [];
let sections = [];

function Body({ className, children }) {
  React.useEffect(() => {
    for (let i = 0; i < stateArray.length; i++) {
      stateArray[i][1](refArray[i].current)
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

  if (sections.length) {
    //ADD REFS and STATES
    sections.forEach((section) => {
      walkAllChildren(section, () => {
        if (section.type?.name === "Child") {
        }
      });
    });
  } else {
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
 * @param {"input" | "button" | "checkbox"} type Type of FormChild
 */
function Child(type, name) {
  let count = stateArray.length;
  console.log(`Number in Array: ${count}`);
  const onChange = (e) => {
    refArray[count].current = e.target.value;
  };
  return { as: Get(type), onChange };
}

function Get(type, name, value) {
  const [state, setState] = React.useState(type === "input" ? "" : false);
  const ref = React.useRef(state);
  stateArray.push([state, setState]);
  refArray.push(ref);
  return { state, setState, ref };
}

export default { Body, Section, Child };
