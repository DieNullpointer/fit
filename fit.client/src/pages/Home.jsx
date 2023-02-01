import Button from "../components/atoms/Button";
import Input from "../components/atoms/Input";
import Navbar from "../components/Navbar";

export default function Home() {
  return (
    <>
      <Navbar pages={[{name: "sign-up"}, {name: "about"}]} profileSettings />
      <div className="flex flex-col space-y-4 m-5">
        <Button text="Button" sharp />
        <Input
          label="username"
          required
          id="in-name"
          purpose="username"
          type="text"
          size={"medium"}
        />
        <Input
          label="password"
          required
          id="in-pw"
          purpose="password"
          type="password"
        />
      </div>
    </>
  );
}
