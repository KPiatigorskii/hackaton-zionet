
import SignalR from '@microsoft/signalr'
import { TwitterRecord, User } from '../entities';
import { SignalRHelper } from "../helpers/SignalRHelper"

import * as https from 'https';
import * as querystring from 'querystring';
export class MssqlAccessorService {
	signalR = require("@microsoft/signalr");
	connection: any;
	url: string;
	messageUser!: User;

	constructor(url: string){
		this.url = url;
	}

	public async connect (token: string){
		// var connection = this.signalR.HubConnectionBuilder.create("http://localhost:5192/Users")
		// .withTransport(this.signalR.TransportEnum.WEBSOCKETS)
		// .build();


		// const connection = new this.signalR.HubConnectionBuilder()
		// .withUrl("http://localhost:5192/Users", { 
		// 	accessTokenFactory: () => token })
		// .configureLogging(this.signalR.LogLevel.Trace)
		// .withAutomaticReconnect()
		// .build()
		// this.connection = connection;

		this.connection = new this.signalR.HubConnectionBuilder() // accessTokenFactory: () => { tokenString },
			.configureLogging(this.signalR.LogLevel.Debug)
			.withUrl(`http://localhost:5192/Users`, {
				headers: {"Authorization":  token },
				skipNegotiation: true,
				transport: this.signalR.HttpTransportType.WebSockets
			  })
			.build();

			this.connection.on("ReceiveGetOne", (data: User) => {
				this.messageUser = data;
			});
			this.connection.on("ReceiveUpdate", (data: User) => {
				this.messageUser = data;
			});

			//
		};

	public async getUser(id: number): Promise<User>
	{
		await this.connection.start()
		await this.connection.invoke("GetOne", id);
		return this.messageUser;
	}

	public async setUser(id: number, user: User)
	{
		await this.connection.start()
		await this.connection.invoke("Update", id, user);
		return this.messageUser;
	}

	// get all records from DB table twitterRecords with already_found = false
	public getTwitterRecords() : TwitterRecord[]
	{
		https.get('http://localhost:7277/api/TwitterRecords', (res) => {
			res.on('data', (data: TwitterRecord[]) => {
				return data;
			});
		});
		return  new Array<TwitterRecord>;
	}

	// update record from DB
	public updateTwitterRecord() : TwitterRecord
	{
		// go to DB and update record
		var t: TwitterRecord = {
			id: 0,
			authorId: 0,
			participantId: 0,
			eventName: '',
			teamName: '',
			enginePort: 0,
			engineCronUuid: '',
			isSearching: false,
			alreadyFound: false
		};
		return t;
	}

}

