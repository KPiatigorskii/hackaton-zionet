import { Scheduler } from "timers/promises";
import { Team } from "../entities";
import { TwitterService } from "./TwitterService";

const cron = require('node-cron');

export class CronService {
    public static cronJobs: Record<string, any> = {};

    public static startCron(twitterService: TwitterService, eventName: string, teamName: string, userId: number, userTwitterId: number): any {
            CronService.cronJobs[userTwitterId] = cron.schedule('*/30 * * * * *',() => twitterService.getTweets(eventName, teamName, userId, userTwitterId));
    }

    public static stopCron(userTwitterId: number): any{
        try {
            CronService.cronJobs[userTwitterId].stop();
            console.log(`cron task ${userTwitterId} was successfully stopped`);
        } catch (error) {
            console.log(`can't stop cron task ${userTwitterId}`);
        }
    }

}
