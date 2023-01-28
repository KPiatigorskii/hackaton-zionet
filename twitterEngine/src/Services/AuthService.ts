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
        const response = await axios.post(process.env.AUTH0_TOKEN_URL, {
          client_id: process.env.AUTH0_TOKEN_CLIENT_ID,
          client_secret: process.env.AUTH0_TOKEN_CLIENT_SECRET,
          audience: process.env.AUTH0_AUDIENCE,
          grant_type: "client_credentials"
        });
        AuthService.token = response.data.access_token;
        console.log("Token received successfuly")
      } catch (error) {
        console.error(error);
      }
    }

    
}