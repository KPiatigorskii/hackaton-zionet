
export class MssqlAccessorService<T> {
	signalR = require("@microsoft/signalr");
	connection: any;
	url: string;
	message!: T;
	messages!: T[];

	constructor(Huburl: string){
		this.url = process.env.MSSQL_ACCESSOR_URL + Huburl
	}

	public async connect (token: string){

		this.connection = new this.signalR.HubConnectionBuilder()
			.configureLogging(this.signalR.LogLevel.Debug)
			.withUrl(this.url, {
				headers: {"Authorization": "Bearer " + token },
				skipNegotiation: true,
				transport: this.signalR.HttpTransportType.WebSockets
			})
			.build();

			this.connection.on("ReceiveGetOne", (data: T) => {
				this.message = data;
			});

			this.connection.on("ReceiveGetAll", (data: T[]) => {
				this.messages = data;
			});

			this.connection.on("ReceiveUpdate", (data: T) => {
				this.message = data;
			});

			this.connection.on("ReceiveCreate", (data: T) => {
				this.message = data;
			});

			this.connection.on("ReceiveDelete", (data: T) => {
				this.message = data;
			});

			//
		};

	public async getOne(id: number): Promise<T>
	{
		await this.connection.start()
		await this.connection.invoke("GetOne", id);
		return this.message;
	}

	public async updateOne(id: number, entity: T)
	{
		await this.connection.start()
		await this.connection.invoke("Update", id, entity);
		return this.message;
	}

	public async create(entity: T) : Promise<T>
	{
		await this.connection.start()
		await this.connection.invoke("Update", entity);
		return this.message;
	}

	public async deletOne(id: number): Promise<T>
	{
		await this.connection.start()
		await this.connection.invoke("Delete", id);
		return this.message;
	}

	public async getAll(): Promise<T[]>
	{
		await this.connection.start()
		await this.connection.invoke("GetAll");
		return this.messages;
	}

	public async getAllWithCondition(filters: Record<string,any>): Promise<T[]>
	{
		await this.connection.start()
		await this.connection.invoke("GetAllWithConditions", filters);
		return this.messages;
	}

	public async getOneWithCondition(filters: Object): Promise<T>
	{
		await this.connection.start()
		await this.connection.invoke("GetOneWithConditions", filters);
		return this.message;
	}

}

