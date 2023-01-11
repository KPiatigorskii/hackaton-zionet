import { Scheduler } from "timers/promises";
import { Team } from "../entities";
import { TwitterService } from "./TwitterService";

const cron = require('node-cron');

export class CronService {
    public static cronJobs: Record<string, any> = {};

    public static startCron(twitterService: TwitterService, eventName: string, teamName: string, userTwitterId: string): any {
            CronService.cronJobs[userTwitterId] = cron.schedule('*/30 * * * * *',() => twitterService.getTweets(eventName, teamName, userTwitterId));
    }

    public static stopCron(userTwitterId: string): any{
        try {
            CronService.cronJobs[userTwitterId].stop();
            console.log(`cron task ${userTwitterId} was successfully stopped`);
        } catch (error) {
            console.log(`can't stop cron task ${userTwitterId}`);
        }
    }

}
