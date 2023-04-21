import axios from "axios";

class APIConstants {
  static BASE_URL = "https://localhost:5001/api";
  static EVENT_URL = this.BASE_URL + "/event";
  static COMPANY_URL = this.BASE_URL + "/company";
  static PACKAGE_URL = this.BASE_URL + "/package";

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

  static async getCompany(guid) {
    let response;
    try {
      response = await axios.get(`${this.COMPANY_URL}/${guid}`)
    } catch (error) {
      return response.data;
    }
  }
}

export default APIConstants;
