
import SignalR from '@microsoft/signalr'
import {SignalRHelper} from "../helpers/SignalRHelper"

  export class SignalRClient {
    signalr = require('@microsoft/signalr');
    loginToken ='eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjJKZ3ZkVm9teDhWcExlbzZYbnhuOSJ9.eyJodHRwOi8vemlvbmV0LWFwaS91c2VyL2NsYWltcy9lbWFpbCI6ImsucGlhdGlnb3Jza2lpQGdtYWlsLmNvbSIsImV2ZW50LmF1dGhvcml6YXRpb24iOnsicm9sZXMiOlsiYWRtaW4iXX0sImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJhZG1pbiJdLCJnaXZlbl9uYW1lIjoiS29uc3RhbnRpbiIsImZhbWlseV9uYW1lIjoiUGlhdGlnb3Jza2lpIiwibmlja25hbWUiOiJrLnBpYXRpZ29yc2tpaSIsIm5hbWUiOiJLb25zdGFudGluIFBpYXRpZ29yc2tpaSIsInBpY3R1cmUiOiJodHRwczovL2xoMy5nb29nbGV1c2VyY29udGVudC5jb20vYS9BRWRGVHA1eU1wSmowdjBtXzhtUDFEOUVtTkN5cTdMZ2hGRWZPTGRVNDJFPXM5Ni1jIiwibG9jYWxlIjoicnUiLCJ1cGRhdGVkX2F0IjoiMjAyMy0wMS0xMFQxNDoyMDozMy4zMTVaIiwiaXNzIjoiaHR0cHM6Ly9kZXYtNWM3eWNiZ2RqeWJzbnFpZi51cy5hdXRoMC5jb20vIiwic3ViIjoiZ29vZ2xlLW9hdXRoMnwxMDYwNTA3ODU1NTQ5MDIyNjk5ODYiLCJhdWQiOiJpa2NMTnc4a2J1Q2d6ZEdlaHJuZU9CZFBVd01SYTY5bCIsImlhdCI6MTY3MzM2MDQzNCwiZXhwIjoxNjczMzk2NDM0LCJzaWQiOiJIVUFNbk5SNXFKSnlReFhiUVJGUUdNSmdZY0JMejg2ViIsIm5vbmNlIjoiNjM4MDg5NTcyMjQ0MjkzOTU3LllXTmtNV0ptWVRjdFpETTRNaTAwTURKakxUaGxOV0l0TW1GaE1USmlZamd6TmpNeU5ETmtNREF5T1RVdFpERmtPQzAwT1dKaUxUbGpOVFl0TkRka01EZ3dZamRqWVRNdyJ9.wfGmLZTmRAVwHOj9Kh7PrUSRIp-xDmnjYUTAWzvyMgd1zms6__iYO4Y4U8sLL-9bh6kOyGF_iUwERdl0aG9pjPyxvLRtZ9EcSAfPmnaBO9uOmzZTtCbfX-xIMXYrToCrrMG-rqt_LQxmTulIRL1YpP3EMKfTHT7i9h2IL9Uk-JJI3rr4v6M0y9vgwe-5VjY7OQkBfp2vOnRIhOnR8BFG9VlFtDBHWyizO4JkjQZ4yDKcuFt0z2vSR0l8_LOWycfr-IVbvQ8NGZADCkl3I4ZT1jF8XwDRj_CVE6aAo4KRe9OGjJvYuq2b5sj6Ige2xsTw57iFk_QRZ6IqqtGjlSxoaQ';
        
    connection : any;
    url: string;
    signalRHelper: SignalRHelper;

    constructor(hubUrl: string) {
      this.url = hubUrl;
      this.signalRHelper = new SignalRHelper(hubUrl)


      
    }

    public callFunction(functionName: string, message: any): Promise<any> {
        return new Promise<any> ((resolve, reject) => { 
          this.signalRHelper.connect(this.url, true, this.loginToken)
         // .then(() => this.connection.invoke(functionName, message))
            .then((result: any) => {
                resolve(result.data);
            })
            .catch((error: any) => {
                reject(error);
            });
        });
    }

  }

