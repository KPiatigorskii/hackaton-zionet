import { TwitterApi } from 'twitter-api-v2';
import * as dotenv from 'dotenv' // see https://github.com/motdotla/dotenv#how-do-i-use-dotenv-with-import
dotenv.config()

  export class TwitterService {
    twitterClient: TwitterApi;

    constructor(){
        this.twitterClient = new TwitterApi({
            appKey: String(process.env.TWITTER_API_KEY) || '',
            appSecret: String(process.env.TWITTER_API_SECRET) || '',
            accessToken: String(process.env.TWITTER_ACCESS_TOKEN) || '',
            accessSecret: String(process.env.TWITTER_ACCESS_TOKEN_SECRET) || '',
          });
        }

        public getAllTweets(queryString: string): Promise<any[]>{
        let result: any[] = [];
        return new Promise<any[]>((resolve, reject) => {
            //SqlHelper.executeQueryArrayResult<localEmployee>(this.errorService, Queries.GetAllEmployee)
            this.twitterClient.v2.get('tweets/search/recent', { query: queryString, max_results: 100 })
            .then((result: any) => {
                resolve(result);
            })
            .catch((error: any) => {
                reject(error)
            });
        });
    }
  }
