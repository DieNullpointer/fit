import axios from "axios";

class APIConstants {
  static BASE_URL = "https://localhost:5001/api";
  static EVENT_URL = this.BASE_URL + "/event";
  static COMPANY_URL = this.BASE_URL + "/company";

  static async getAllEvents() {
    let response;
    try {
      response = await axios.get(this.EVENT_URL)
      console.log(response);
    } catch (error) {}
    return response.data;
  }
}

export default APIConstants;
