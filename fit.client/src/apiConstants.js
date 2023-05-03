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

  static async getCompany(guid) {
    let response;
    try {
      response = await axios.get(`${this.COMPANY_URL}/${guid}`)
    } catch (error) {
      return response.data;
    }
  }
  
  static async registerCompany(payload) {
    let response;
    try {
      response = await axios.post(`${this.COMPANY_URL}/register`, payload);
    } catch (error) {
      console.log(error);
      return response.errors;
    }
    return true;
   }
}
export default APIConstants;
