import * as schedule from "node-schedule";
import {  } from "node-schedule";
class CronSchedule {
    schedule: any;

    constructor(func: Function) {
        this.schedule.scheduleJob('42 * * * *', function(){
            console.log('The answer to life, the universe, and everything!');
            func();
        });
    }

    public gracefulShutdown(func: Function): any {
        schedule.gracefulShutdown();
    }

}
