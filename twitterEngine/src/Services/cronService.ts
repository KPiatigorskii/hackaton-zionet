import { Scheduler } from "timers/promises";
import { Team } from "../entities";
import { TwitterService } from "./TwitterService";
import { TwitterRecord } from './../entities';
import {v4 as uuidv4} from 'uuid';

const cron = require('node-cron');

export class CronService {
    public static cronJobs: Record<string, any> = {};

    public static startCron(record : TwitterRecord, token: string){
        let newUuid = uuidv4();
        CronService.cronJobs[newUuid] = cron.schedule('*/30 * * * * *', () => 
            TwitterService.getTweets(record, token));
        return newUuid;

    }

    public static stopCron(engineCronUuid: string, token: string): any {
        try {
            CronService.cronJobs[engineCronUuid].stop();
            console.log(`cron task ${engineCronUuid} was successfully stopped`);
        } catch (error) {
            console.log(`can't stop cron task ${engineCronUuid}`);
        }
    }

    public static getCronByUuid(cronUuid: string, token: string){
        if (this.isCronRunning(cronUuid))
            return true;
        else
            return false;
    }

    public static isCronRunning(engineCronUuid: string): boolean {
        const job = CronService.cronJobs[engineCronUuid];
        return job != undefined;
    }
}
