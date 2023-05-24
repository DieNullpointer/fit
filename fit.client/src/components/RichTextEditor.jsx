import { useState } from "react";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";

export default function RichTextEditor({placeholder}) {
    
    const [editorHtml, setEditorHtml] = useState("");

  return (
    <ReactQuill
      theme={"snow"}
      onChange={(html) => setEditorHtml(() => {
        sessionStorage.setItem("editorHtml", html);
        return html;
      })}
      value={editorHtml}
      modules={{
        toolbar: [
          [{ header: "1" }, { header: "2" }],
          [{ size: [] }],
          ["bold", "italic", "underline", "strike", "blockquote"],
          [
            { list: "ordered" },
            { list: "bullet" },
            { indent: "-1" },
            { indent: "+1" },
          ],
          ["link"],
          ["clean"],
        ],
        clipboard: {
          // toggle to add extra line breaks when pasting HTML:
          matchVisual: false,
        },
      }}
      formats={[
        "header",
        "size",
        "bold",
        "italic",
        "underline",
        "strike",
        "blockquote",
        "list",
        "bullet",
        "indent",
        "link",
      ]}
      placeholder={placeholder}
    />
  );
}