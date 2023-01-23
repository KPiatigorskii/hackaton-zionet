
import axios from 'axios';
import { PORT } from '../../app';
import { CronService } from '../services/CronService';
import { MssqlAccessorService } from '../services/MssqlAccessorService';
import { TwitterService } from '../services/TwitterService';
import { TwitterRecord } from './../entities';

export class InitializeController
{
    static records: TwitterRecord[];

    public static getAllActualRecords = async () => {
        let token = process.env.TEST_TOKEN; //TODO make real authorization
        var mssql = new MssqlAccessorService<TwitterRecord>("/TwitterRecords");
        mssql.connect( String(token));  
        const filter : Record<string, any> = {
            "AlreadyFound": false,
        }
        const twitterUnsortedRecords: TwitterRecord[] = await mssql.getAllWithCondition(filter);
        twitterUnsortedRecords.forEach((record: TwitterRecord) => {
            this.CheckRecordJobs(record);
        });

    }

    public static CheckRecordJobs = async (record: TwitterRecord) => {
        let token = process.env.TEST_TOKEN; //TODO make real authorization
        var mssql = new MssqlAccessorService<TwitterRecord>("/TwitterRecords");
        mssql.connect( String(token)); 
        var needUpdate: boolean = false;
        if (PORT != record.enginePort && record.isSearching){ // remote cron fails
            try {
                // const response = await axios.get(`${process.env.HOST_URL}:${record.enginePort}/CronSchedule/${record.engineCronUuid}/status`);
                // if (response.status === 200 && response.data === "running") {
                //     needUpdate = false;
                // } else {
                //     needUpdate = true;
                // }
            } catch (error) {
                console.error(error);
                needUpdate = true;
            }
        }
        if (PORT == record.enginePort && record.isSearching){ // our cron fails
            needUpdate = !CronService.isCronRunning(record.engineCronUuid);
        }
        if (needUpdate || !record.isSearching){
            record.enginePort = PORT;
            record.isSearching = true;
            record.engineCronUuid = CronService.startCron(record, process.env.TEST_TOKEN || "");
            //console.log(`start cron with uuid ${record.engineCronUuid}`);
            mssql.updateOne(record.id, record)
        }
    }
}