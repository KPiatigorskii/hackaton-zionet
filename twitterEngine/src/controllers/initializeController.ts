import axios from 'axios';
import { CronService } from '../services/cronService';
import { MssqlAccessorService } from '../services/MssqlAccessorService';
import { TwitterRecord } from './../entities';
import { PORT } from '../../app';

export class InitializeController
{
    static port: number;

    public static setPort(PORT: number){
        this.port = PORT;
    }

    public static async getAllActualRecords(){
        var mssql = new MssqlAccessorService<TwitterRecord>("/TwitterRecords");
        console.log("Getting records from DB for searching....")
        mssql.connect();  
        const filter : Record<string, any> = {
            "AlreadyFound": false,
        }
        const twitterUnsortedRecords: TwitterRecord[] = await mssql.getAllWithCondition(filter);
        console.log(`Get ${twitterUnsortedRecords.length} records from DB`)
        twitterUnsortedRecords.forEach((record: TwitterRecord) => {
            console.log(`Search in jobs record with uuid=${record.engineCronUuid} and id=${record.id}...`);
            this.CheckRecordJobs(record);
        });

    }

    public static async CheckRecordJobs(record: TwitterRecord){
        let token = process.env.TEST_TOKEN; //TODO make real authorization
        var mssql = new MssqlAccessorService<TwitterRecord>("/TwitterRecords");
        mssql.connect(); 
        var needUpdate: boolean = false;
        if (InitializeController.port != record.enginePort && record.isSearching){ // remote cron fails
            console.log(`Get remote job status from port ${record.enginePort}`)
            try {
                const response = await axios.get(`${process.env.HOST_URL}:${record.enginePort}/CronSchedule/${record.engineCronUuid}/status`);
                console.log(`Status of remote job is ${response.data}`);
                if (response.status === 200 && response.data.message === true) {  
                    needUpdate = false;
                } else {
                    needUpdate = true;
                }
            } catch (error) {
                console.error(error);
                needUpdate = true;
            }
        }
         if (InitializeController.port == record.enginePort && record.isSearching){ // our cron fails
            console.log(`Get local job status from our server with uuid=${record.engineCronUuid}`);
            var cronStatus = CronService.isCronRunning(record.engineCronUuid);
            needUpdate = !cronStatus;
            console.log(`Status of local job is ${cronStatus}`);
        }
        if (needUpdate || !record.isSearching){
            console.log(`Starting new cron job...`);
            record.enginePort = InitializeController.port;
            record.isSearching = true;
            record.engineCronUuid = CronService.startCron(record);
            console.log(`Job successfully started with uuid=${record.engineCronUuid}`);
            mssql.updateOne(record.id, record)
            console.log(`Job successfully updated on DB`);
        }
    }
}