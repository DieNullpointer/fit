import axios from "axios";

class APIConstants {
  
  static EVENT_URL = "/event";
  static COMPANY_URL = "/company";
  static PACKAGE_URL = "/package";
  
  static async getAllEvents() {
    let response;
    try {
      response = await axios.get(this.EVENT_URL);
    } catch (error) {}
    return response.data;
  }

  static async getAllPackages() {
    let response;
    try {
      response = await axios.get(this.PACKAGE_URL);
    } catch (error) {}
    return response.data;
  }
}
export default APIConstants;
