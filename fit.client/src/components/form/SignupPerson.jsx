import Paper from "../atoms/Paper";
import Input from "../atoms/Input";
import Checkbox from "../atoms/Checkbox";

export function SignupPerson({
  number,
  mainPartnerDisabled,
  disabled,
  onChange,
}) {
  //not using form component due to reworks
  let data = { _intern: number };
  

  const sharedProps = (registry) => {
    return {
      disabled: disabled,
      onChange: (e) => {
        Object.defineProperty(data, registry, {
          value: e.target.value,
          enumerable: true,
          writable: true,
        });
        console.log(`changed input of ${registry} to ${e.target.value}`);
        onChange?.(number, data, e);
      },
    };
  };

  return (
    <form key={"paper-key-person-" + number}>
      <Paper elevation={3}>
        <div className="px-3" id={"person-" + number}>
          <div className="grid md:grid-cols-3 grid-flow-column">
            <Input
              id="in-title"
              label="Titel"
              required
              purpose={"text"}
              full
              className="m-3"
              {...sharedProps("title")}
            />
            <Input
              id="in-firstname"
              label="Vorname"
              required
              purpose={"text"}
              full
              className="m-3"
              {...sharedProps("firstname")}
            />
            <Input
              id="in-lastname"
              label="Nachname"
              required
              purpose={"text"}
              full
              className="m-3"
              {...sharedProps("lastname")}
            />
          </div>
          <div className="grid grid-cols-1 md:grid-cols-2">
            <div className="pr-3">
              <Input
                id="in-telnr"
                label="Telefonnummer"
                required
                purpose={"text"}
                full
                {...sharedProps("telnr")}
              />
              <Input
                id="in-mobilnr"
                label="Mobilnummer"
                purpose={"text"}
                full
                {...sharedProps("mobilnr")}
              />
              <Input
                id="in-email"
                label="Email"
                required
                purpose={"text"}
                full
                {...sharedProps("email")}
              />
            </div>
            <div>
              <Input
                id="in-function"
                label="Funktion i. d. Firma"
                required
                purpose={"text"}
                full
                {...sharedProps("function")}
              />
              <div className="-mt-6">
                <Checkbox
                  label="Hauptansprechpartner"
                  onChange={(e) => {
                    //mainPersonRef.current = number;
                    //console.log("Set to main partner " + number);
                  }}
                  disabled={mainPartnerDisabled}
                />
              </div>
            </div>
          </div>
        </div>
      </Paper>
    </form>
  );
}
