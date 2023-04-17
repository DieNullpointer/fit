import Form from "./Form";
import Paper from "../atoms/Paper";
import Input from "../atoms/Input";
import Checkbox from "../atoms/Checkbox";

export function SignupPerson({ number, mainPartnerDisabled }) {
  return (
    <Form.Body key={"paper-key-person-" + number}>
      <Paper elevation={3} >
        <Form.Section className="px-3" id={"person-" + number}>
          <div className="grid md:grid-cols-3 grid-flow-column">
            <Input
              id="in-title"
              label="Titel"
              required
              purpose={"text"}
              full
              className="m-3"
              {...Form.Child("input", "title")}
            />
            <Input
              id="in-firstname"
              label="Vorname"
              required
              purpose={"text"}
              full
              className="m-3"
              {...Form.Child("input", "firstname")}
            />
            <Input
              id="in-lastname"
              label="Nachname"
              required
              purpose={"text"}
              full
              className="m-3"
              {...Form.Child("input", "lastname")}
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
                {...Form.Child("input", "telnr")}
              />
              <Input
                id="in-mobilnr"
                label="Mobilnummer"
                purpose={"text"}
                full
                {...Form.Child("input", "mobilnr")}
              />
              <Input
                id="in-email"
                label="Email"
                required
                purpose={"text"}
                full
                {...Form.Child("input", "email")}
              />
            </div>
            <div>
              <Input
                id="in-function"
                label="Funktion i. d. Firma"
                required
                purpose={"text"}
                full
                {...Form.Child("input", "function")}
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
        </Form.Section>
      </Paper>
    </Form.Body>
  );
}

export function getExport() {
  Form.getExport();
}

export function isReady() {
    const exportObj = getExport();
    if(exportObj) {
        var allGood = true;
        console.log(Object.entries(exportObj));
        Object.entries(exportObj).forEach((val, idx) => {
            if(val === "") allGood = false; 
        })
    }
}
