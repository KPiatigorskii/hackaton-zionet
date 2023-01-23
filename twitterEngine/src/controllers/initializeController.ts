
import { CronService } from '../services/CronService';
import { MssqlAccessorService } from '../services/MssqlAccessorService';
import { TwitterService } from '../services/TwitterService';
import { TwitterRecord } from './../entities';

export class InitializeController
{
    static records: TwitterRecord[];
    public static async getAllActualRecords() : Promise<TwitterRecord[]> {
        
        //return;
    let token = process.env.TEST_TOKEN;
    let mssql = new MssqlAccessorService<TwitterRecord>("/TwitterRecords");
    let twitterService = new TwitterService();
    mssql.connect( String(token));
    this.records = await mssql.getAll();
    let twitterSortedRecords: TwitterRecord[] =await twitterService.getRecordsWithInvalidInstances(this.records);
    twitterSortedRecords.forEach((record: TwitterRecord) => {
      CronService.startCron(record, twitterService,"");
    });
    return  this.records;

    }
    
}