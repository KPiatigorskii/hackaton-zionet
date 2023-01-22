import { Scheduler } from "timers/promises";
import { Team } from "../entities";
import { TwitterService } from "./TwitterService";
import { TwitterRecord } from './../entities';

const cron = require('node-cron');

export class CronService {
    public static cronJobs: Record<string, any> = {};

    // public static startCron(twitterService: TwitterService, eventName: string, teamName: string, userId: number, userTwitterId: number, token: string): any {
    //     CronService.cronJobs[userTwitterId] = cron.schedule('*/30 * * * * *', () => twitterService.getTweets(eventName, teamName, userId, userTwitterId, token));
    // }

    public static startCron(record : TwitterRecord, twitterService: TwitterService, token: string){
        CronService.cronJobs[record.engineCronUuid] = cron.schedule('*/30 * * * * *', () => 
            twitterService.getTweets(record.eventName, record.teamName, record.participantId, record.authorId, token));
    }

    public static stopCron(engineCronUuid: number, token: string): any {
        try {
            CronService.cronJobs[engineCronUuid].stop();
            console.log(`cron task ${engineCronUuid} was successfully stopped`);
        } catch (error) {
            console.log(`can't stop cron task ${engineCronUuid}`);
        }
    }

    public static getCronByUuid(cronUuid: string, token: string)
    {

    }

}
