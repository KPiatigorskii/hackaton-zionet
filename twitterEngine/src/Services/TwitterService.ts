import { TwitterApi } from 'twitter-api-v2';


  export class TwitterService {
    twitterClient: TwitterApi;

    constructor(){
        this.twitterClient = new TwitterApi({
            appKey: 'M4TuwEsziKK5gZS1uGujpsuHM',
            appSecret: 'i4WkYEPn27YUohbfPRsi3DStkyFV45rHTYC4M13Rp0i5XLXNYV',
            accessToken: '1606734720569692160-IV0JnoYdvyARYLXzXC1qv89uy36dT7',
            accessSecret: 'kw6RrolXZrrebqX54GLSdeCzjGrsk2coU7vBQ3toYCjyh',
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
