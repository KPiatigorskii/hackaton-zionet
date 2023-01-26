var axios = require("axios").default;
const jwt = require("jsonwebtoken");

export class AuthService {
    private static token: string;
  
    constructor() {
        AuthService.token = '';
    }

    static getToken(){
        return AuthService.token;
    }
  
    static async receiveToken() {
      try {
        const response = await axios.post('https://dev-5c7ycbgdjybsnqif.us.auth0.com/oauth/token', {
          client_id: "Esu7PsT3vnUh4SktOBAuBliM3Fes6ttl",
          client_secret: "0-CFh6odWI7CXTdCKi6taSxw4YkXAZ6xRnWajWpkhyspemkJuEHUV2CjvG8du2UG",
          audience: "https://zionet-api.com",
          grant_type: "client_credentials"
        });
        AuthService.token = response.data.access_token;
        console.log("Token received successfuly")
      } catch (error) {
        console.error(error);
      }
    }

    
}