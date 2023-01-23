
import axios from 'axios';
import { PORT } from '../../app';
import { CronService } from '../services/CronService';
import { MssqlAccessorService } from '../services/MssqlAccessorService';
import { TwitterService } from '../services/TwitterService';
import { TwitterRecord } from './../entities';

export class InitializeController
{
    static mssql: MssqlAccessorService<TwitterRecord>;

    constructor() {
        InitializeController.mssql = new MssqlAccessorService<TwitterRecord>("/TwitterRecords");
        
    }
    static records: TwitterRecord[];
    public static async getAllActualRecords() : Promise<TwitterRecord[]> {

        let token = process.env.TEST_TOKEN; //TODO make real authorization

        let twitterService = new TwitterService();
        this.mssql.connect( String(token));
        const filter : Record<string, any> = {
            "AlreadyFound": false,
        }
        const twitterUnsortedRecords: TwitterRecord[] = await InitializeController.mssql.getAllWithCondition(filter);
        twitterUnsortedRecords.forEach((record: TwitterRecord) => {
            this.CheckRecordJobs(record);
        });
    return  this.records;

    }

    public static CheckRecordJobs(record: TwitterRecord){
        var needUpdate: boolean = false;
        if (PORT != record.enginePort && record.isSearching){ // remote cron fails
            needUpdate = true;//axios(`${process.env.HOST_URL}:${record.enginePort}/CronSchedule/${record.engineCronUuid}`); // ping service
        }
        if (PORT == record.enginePort && record.isSearching){ // our cron fails
            needUpdate = !CronService.isCronRunning(record.engineCronUuid);
        }
        if (needUpdate){
            record.engineCronUuid = CronService.startCron(record, process.env.TEST_TOKEN || "");
            this.updateRecord(record);
        }
        
    }

    public static updateRecord(record: TwitterRecord){
        record.enginePort = PORT;
        record.isSearching = true;
        InitializeController.mssql.updateOne(record.id, record)
    }
    
}