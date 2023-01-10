import { Scheduler } from "timers/promises";
import { TwitterService } from "./TwitterService";

const cron = require('node-cron');

export class CronService {
    public cronJobs: Record<number, any> = {};

    public startCron(twitterService: TwitterService, query: string, team_id:number): any {
            this.cronJobs[team_id] = cron.schedule('*/10 * * * * *',() => twitterService.getTweets(query));
    }

    public stopCron(team_id: number): any{
        this.cronJobs[team_id].stop();
    }

}
