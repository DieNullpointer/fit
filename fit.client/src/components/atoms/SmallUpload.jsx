import Button from "./Button";

export default function SmallUpload({ label, helpText, id, action, multiple }) {
  return (
    <form action={action} className="flex justify-between items-center">
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
        multiple={multiple}
      ></input>
      <p className="ml-1 text-sm text-grey-800 opacity-75" id={id + "_helper"}>{helpText}</p>
      </div>
      <Button submit style={{maxWidth: '200px', maxHeight: '30px', minWidth: '30px', minHeight: '30px'}} text={`Datei${multiple ? "en" : ""} Absenden`} id={id + "_submit"} />
    </form>
  );
}
