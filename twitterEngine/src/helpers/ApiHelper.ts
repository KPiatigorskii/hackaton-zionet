import { Request, Response, NextFunction } from 'express';
import axios, { AxiosResponse } from 'axios';

export class ApiHelper {
     public https;
     public axios;

    constructor() {
        this.https = require('https');
        this.axios = require('axios');
    }

    public async UserIsTwit(userId: number) {
    try {
        const response = await axios.post(`https://localhost:7277/api/UsersDb/${userId}/isTwit`, { 
            httpsAgent: new this.https.Agent({ rejectUnauthorized: false }) 
        });
        console.log(response.data);
        } catch (error) {
            console.log(error);
        }
    }
}
