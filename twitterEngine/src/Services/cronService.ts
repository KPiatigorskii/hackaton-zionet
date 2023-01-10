import { TwitterService } from "./TwitterService";

const cron = require('node-cron');

export class CronService {

    public startCron(twitterService: TwitterService,query: string): any {
            cron.schedule('*/10 * * * * *',() => twitterService.getAllTweets(query));
    }

    public stopCron(): Promise<any> {
        return new Promise<any>((resolve, reject) => {
            cron.gracefulShutdown()
            .then((result: any) => {
                resolve("crone stop successfully!");
            })
            .catch((error: any) => {
                reject(error)
            });
        });
    }

}
