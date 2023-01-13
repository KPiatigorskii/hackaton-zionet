import { TwitterApi } from 'twitter-api-v2';
import { Team } from '../entities'
import { CronService } from './CronService'
import { ApiHelper } from '../helpers/ApiHelper';
import * as dotenv from 'dotenv' // see https://github.com/motdotla/dotenv#how-do-i-use-dotenv-with-import
dotenv.config()

export class TwitterService {
	twitterClient: TwitterApi;
	apiHelper: ApiHelper;

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

	public getTweets(eventName: string, teamName: string, userId: number, userTwitterId: number): Promise<any> {
		return new Promise<any>((resolve, reject) => {
			let minutesInterval = 2;
			let currentDate = new Date();
			currentDate.setMinutes(currentDate.getMinutes() - minutesInterval);
			let formattedDate = currentDate.toISOString();
			console.log(formattedDate);
			const result = this.twitterClient.v2.search(eventName + " " + teamName, { 'expansions': 'author_id', 'start_time': formattedDate })
				.then((result: any) => {
					console.log("==============================================")
					try {
						var isMatched = false;
						for (const tweet of result.tweets) {
							console.log("author_id: " + tweet.author_id);
							console.log("text: " + tweet.text.substring(0, 20));
							if (tweet.author_id == userTwitterId) {// get match from team members
								// find eventid teamid userid, refre
								isMatched = true;
							}
						}
						if (isMatched) {
							this.apiHelper.UserIsTwit(userId);
							CronService.stopCron(userTwitterId);
						}
					} catch (error) {
						console.log("smthing goes wrong.....");
					}
					resolve(result.data);
				})
				.catch((error: any) => {
					console.log(error);
					reject(error)
				});
		});
	}

	public sendTweet(tweetString: string){
		return new Promise<any>((resolve, reject) => {
			this.twitterClient.v1.tweet('This tweet was written by a bot').then((val) => {
				console.log(val)
				console.log("success")
			}).catch((err) => {
				console.log(err)
			})
		});
	}
}
