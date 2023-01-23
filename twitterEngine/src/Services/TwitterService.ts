import { TwitterApi } from 'twitter-api-v2';
import { Team, TwitterRecord } from '../entities'
import { CronService } from './CronService'
import { ApiHelper } from '../helpers/ApiHelper';
import * as dotenv from 'dotenv' // see https://github.com/motdotla/dotenv#how-do-i-use-dotenv-with-import
dotenv.config()


export class TwitterService {
	twitterClient: TwitterApi;
	apiHelper: ApiHelper;
	static twitterClient: any;
	static apiHelper: any;

	constructor() {
		this.apiHelper = new ApiHelper();
        this.twitterClient = new TwitterApi({
            appKey: String(process.env.TWITTER_API_KEY) || '',
            appSecret: String(process.env.TWITTER_API_SECRET) || '',
            accessToken: String(process.env.TWITTER_ACCESS_TOKEN) || '',
            accessSecret: String(process.env.TWITTER_ACCESS_TOKEN_SECRET) || '',
          });
		if (this.twitterClient == null) {
			throw new Error("Can't authorize in twitter");

		}
	}

	public static async getTweets(record: TwitterRecord, token: string): Promise<any> {
			let minutesInterval = 2;
			let currentDate = new Date();
			currentDate.setMinutes(currentDate.getMinutes() - minutesInterval);
			let formattedDate = currentDate.toISOString();
			console.log(formattedDate);
			console.log(`Searching tweets in proccess ${record.engineCronUuid} with data: 
			${record.teamName} 
			${record.eventName} 
			${record.authorId}`);
			const result =await this.twitterClient.v2.search(record.eventName + " " + record.teamName,
							{ 'expansions': 'author_id', 'start_time': formattedDate })
			console.log("==============================================")
			try {
				var isMatched = false;
				for (const tweet of result.tweets) {
					console.log("author_id: " + tweet.author_id);
					console.log("text: " + tweet.text.substring(0, 20));
					if (tweet.author_id == String(record.authorId)) {// get match from team members
						isMatched = true;
					}
				}
				if (isMatched) {
					console.log("Found a match!");
					this.apiHelper.UserIsTwit(record.participantId);
					console.log(`Stopping cron job with uuid=${record.engineCronUuid}`);
					CronService.stopCron(record.engineCronUuid, token);
					return "Tweet is stopped";
				}
			} catch (error) {
				console.log("smthing goes wrong.....");
			}
	}

	public sendTweet(tweetString: string){
		return new Promise<any>((resolve, reject) => {
			this.twitterClient.v1.tweet(tweetString).then((val) => {
				console.log("success");
				resolve("success");
			}).catch((error) => {
				console.log(error)
				reject(error)
			})
		});
	}
}
