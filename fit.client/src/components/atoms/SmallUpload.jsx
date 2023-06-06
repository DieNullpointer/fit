import Button from "./Button";
import { useState } from "react";
import { Typography } from "@mui/material";

export default function SmallUpload({ label, helpText, id, action, multiple, backendName }) {
  const [hasFiles, setHasFiles] = useState(false);

  return (
    <form action={action} className="flex justify-between items-center" method="POST" encType="multipart/form-data">
      <div className="w-3/4">
        <label
          className="mb-1 block text-base font-medium text-gray-900"
          htmlFor={id}
        >
          {label}
        </label>
        <input
          className="block w-full text-sm text-white border border-white rounded-lg cursor-pointer bg-primary focus:outline-none"
          id={id}
          type="file"
          name={backendName || "formFile"}
          multiple={multiple}
          onChange={() => setHasFiles(true)}
        ></input>
        <p className="ml-1 text-sm text-grey-800 opacity-75" id={id + "_helper"}>{helpText}</p>
      </div>
      <div>
        <Button disabled={!hasFiles} submit style={{maxWidth: '200px', maxHeight: '30px', minWidth: '30px', minHeight: '30px'}} text={`Datei${multiple ? "en" : ""} Absenden`} id={id + "_submit"} />
      </div>
      </form>
  );
}
