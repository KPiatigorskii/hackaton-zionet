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

        public getTweets(queryString: string): Promise<any> {
          return new Promise<any> ((resolve, reject) => {
              const result = this.twitterClient.v2.get('tweets/search/recent', { query: queryString, max_results: 10 })
          .then((result: any) => {
            console.log("==============================================")
            result.data.forEach((element: { id: string; text: string; }) => {
              console.log(`queryString: ${queryString}`);
              console.log(`id: ${element.id}`);
              console.log(`text: ${element.text.slice(0, 50)}`);
            });
              resolve(result.data);
        })
        .catch((error: any) => {
            reject(error)
        });
      });
  }
}
