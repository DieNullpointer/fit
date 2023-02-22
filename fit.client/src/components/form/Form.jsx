import React from "react";

export default function Form({ className, children }) {
  let stateArray = [];
  let refArray = [];

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

  walkAllChildren(<>{children}</>, (e, parents) => {console.log(e.props?.id);})
  //TODO

  return <form className={className}>{children}</form>;
}
