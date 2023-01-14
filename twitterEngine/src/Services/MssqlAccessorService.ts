
import SignalR from '@microsoft/signalr'
import { SignalRHelper } from "../helpers/SignalRHelper"

export class SignalRClient {
	signalR = require("@microsoft/signalr");
	loginToken = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjJKZ3ZkVm9teDhWcExlbzZYbnhuOSJ9.eyJodHRwOi8vemlvbmV0LWFwaS91c2VyL2NsYWltcy9lbWFpbCI6InNwcnV0LnNob3dAZ21haWwuY29tIiwiZXZlbnQuYXV0aG9yaXphdGlvbiI6eyJyb2xlcyI6WyJhZG1pbiIsInBhcnRpY2lwYW50Il19LCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiYWRtaW4iLCJwYXJ0aWNpcGFudCJdLCJnaXZlbl9uYW1lIjoiS29uc3RhbnRpbiIsImZhbWlseV9uYW1lIjoiUHlhdGlnb3Jza3kiLCJuaWNrbmFtZSI6InNwcnV0LnNob3ciLCJuYW1lIjoiS29uc3RhbnRpbiBQeWF0aWdvcnNreSIsInBpY3R1cmUiOiJodHRwczovL2xoMy5nb29nbGV1c2VyY29udGVudC5jb20vYS9BRWRGVHA0b2dVQjFTSXhDaFh4VWdTY3pvclIzR3BoYVFUbDFoeU8xenJlZT1zOTYtYyIsImxvY2FsZSI6InJ1IiwidXBkYXRlZF9hdCI6IjIwMjMtMDEtMTRUMTA6MDA6NDMuODAwWiIsImlzcyI6Imh0dHBzOi8vZGV2LTVjN3ljYmdkanlic25xaWYudXMuYXV0aDAuY29tLyIsInN1YiI6Imdvb2dsZS1vYXV0aDJ8MTE0MzM0NTYwMDUwNzg5ODM2OTg2IiwiYXVkIjoiaWtjTE53OGtidUNnemRHZWhybmVPQmRQVXdNUmE2OWwiLCJpYXQiOjE2NzM2OTA0NDQsImV4cCI6MTY3MzcyNjQ0NCwic2lkIjoiYWV5a25sN1RHV1lzVVFYZ1JjSURfaVU3SlIzdm5KZjQiLCJub25jZSI6IjYzODA5Mjg3MjM2MjU5NTQwOC5PR0kwWkdRMVltUXRaREZoWkMwMFlqTTNMV0UzWTJJdFlqY3laVEppWkdGaVlUazVNelE1WWpabE1tRXRObVExWkMwME5UZzFMV0kwWmprdFlUZzFOR1l4TkRnek1UTTQifQ.HdOlyenL65pm2YO-sO1jmcnqYuytprFXZuzZXyK7fmNTs1QI53EG27lhQPkGKM7ldmj5z43ckX4b7BYyT1R7LGMddwrANwnuLeOr9vdzqS_5qqQbls35UzblRY3d5bjq9axNUpG4xFNojuKattR-AOsPzldJNguqrNgJieJDfUF-kq0hlHPt3NrnLx7fuEfTU6LC9Ch-KTio47vW_f_6TCW3nAEtqhWxCQSO3kg_GMPFgQt4fGOq8o97t707bw1g_mGlX387_3xihNuiN0Rd6WadUd4q6zh33trVb5TQsHfwCsNHXUrIHid50Jbv2M7eX216WBaCvwRF1fduTcCc8Q';
	connection: any;
	url: string;

	constructor(hubUrl: string) {
		this.url = hubUrl;
		this.connection = new this.signalR.HubConnectionBuilder() // accessTokenFactory: () => { tokenString },
			.configureLogging(this.signalR.LogLevel.Debug)
			.withUrl(`${hubUrl}`, {
				headers: {"Authorization": "Bearer "+this.loginToken },
				skipNegotiation: true,
				transport: this.signalR.HttpTransportType.WebSockets
			  })
			.build();
		this.connection.on("ReceiveGetOne", (data: any) => {
			console.log(data);
		});
	}

	public testFunction(){
		this.connection.start()
    		.then(() => this.connection.invoke("GetOne", 2));
	}

}

