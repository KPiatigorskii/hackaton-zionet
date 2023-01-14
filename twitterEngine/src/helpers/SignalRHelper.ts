
import { HubConnection, HubConnectionBuilder, HubConnectionState, IHttpConnectionOptions, HttpTransportType } from '@microsoft/signalr';

export class SignalRHelper {
	signalR = require("@microsoft/signalr");
	private hubConnection!: HubConnection;
	baseUrl: string;
	connection: any;

	constructor(baseUrl: string) {
		this.baseUrl = baseUrl;
	}

	async connect(tokenString: string = ''): Promise<void> {

		this.connection = new this.signalR.HubConnectionBuilder()
			.configureLogging(this.signalR.LogLevel.Debug)
			.withUrl(`${this.baseUrl}`, {
				headers: {"Authorization": "Bearer " + tokenString },
				skipNegotiation: true,
				transport: this.signalR.HttpTransportType.WebSockets
			})
			.build();

			this.connection.on("ReceiveGetOne", (data: any) => {
				return data;
			});
			this.connection.on("ReceiveGetAll", (data: any[]) => {
				return data;
			});

		

		return this.connection.start()
			.then(() => {
				if (this.isConnected()) {
					console.log('SignalR: Connected to the server: ' + this.baseUrl);
				}
			})
			.catch((err: any) => {
				console.error('SignalR: Failed to start with error: ' + err.toString());
			})
	}

	async define(methodName: string, newMethod: (...args: any[]) => void): Promise<void> {
		if (this.connection) {
			this.connection.on(methodName, newMethod);
		}
	}

	async invoke(methodName: string, ...args: any[]): Promise<any> {
		if (this.isConnected()) {
			return this.connection.invoke(methodName, ...args);
		}
	}

	disconnect(): void {
		if (this.isConnected()) {
			this.connection.stop();
		}
	}

	isConnected(): boolean {
		return this.connection && this.connection.state === HubConnectionState.Connected;
	}
}