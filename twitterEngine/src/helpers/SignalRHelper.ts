
import { HubConnection, HubConnectionBuilder, HubConnectionState, IHttpConnectionOptions, HttpTransportType } from '@microsoft/signalr';

export class SignalRHelper {

  
    private hubConnection!: HubConnection;
    baseUrl: string;
  
    constructor(baseUrl: string) {
        this.baseUrl = baseUrl;
     }
  
    async connect(url: string, withToken: boolean, tokenString: string = ''): Promise<void> {
  
      const builder = new HubConnectionBuilder();
      if (!withToken) {
        builder.withUrl(url, {
          skipNegotiation: true,
          transport: HttpTransportType.WebSockets
        });
      } else {
        builder.withUrl(url, {
          accessTokenFactory: () => { tokenString },
          skipNegotiation: true,
          transport: HttpTransportType.WebSockets
        } as IHttpConnectionOptions);
      }
      this.hubConnection = builder.withAutomaticReconnect().build();
  
      return this.hubConnection.start()
        .then(() => {
          if (this.isConnected()) {
            console.log('SignalR: Connected to the server: ' + url);
          }
        })
        .catch(err => {
          console.error('SignalR: Failed to start with error: ' + err.toString());
        });
    }
  
    async define(methodName: string, newMethod: (...args: any[]) => void): Promise<void> {
      if (this.hubConnection) {
        this.hubConnection.on(methodName, newMethod);
      }
    }
  
    async invoke(methodName: string, ...args: any[]): Promise<any> {
      if (this.isConnected()) {
        return this.hubConnection.invoke(methodName, ...args);
      }
    }
  
    disconnect(): void {
      if (this.isConnected()) {
        this.hubConnection.stop();
      }
    }
  
    isConnected(): boolean {
      return this.hubConnection && this.hubConnection.state === HubConnectionState.Connected;
    }
  }