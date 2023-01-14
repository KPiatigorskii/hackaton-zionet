
import SignalR from '@microsoft/signalr'
import { SignalRHelper } from "../helpers/SignalRHelper"

export class MssqlAccessorService {
	signalR = require("@microsoft/signalr");
	connection: any;
	url: string;
	messageUser: any;

	constructor(url: string){
		this.url = url;
	}

	public async connect (token: string): Promise<void> {
		this.connection = new this.signalR.HubConnectionBuilder() // accessTokenFactory: () => { tokenString },
			.configureLogging(this.signalR.LogLevel.Debug)
			.withUrl(`${this.url}`, {
				headers: {"Authorization":  token },
				skipNegotiation: true,
				transport: this.signalR.HttpTransportType.WebSockets
			  })
			.build();
			this.connection.on("ReceiveGetOne", (data: any) => {
				this.messageUser = data;
			});
		};

	public async getUser(id: number): Promise<any> {
		{
			await this.connection.start()
    		await this.connection.invoke("GetOne", id);
			return this.messageUser
		}
	}
}

