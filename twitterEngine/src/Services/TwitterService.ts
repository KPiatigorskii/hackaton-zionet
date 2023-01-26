import { TweetSearchRecentV2Paginator, TwitterApi } from 'twitter-api-v2';
import { Team, TwitterRecord, User } from '../entities'
//import { CronService } from './CronService'
import { ApiHelper } from '../helpers/ApiHelper';
import * as dotenv from 'dotenv' // see https://github.com/motdotla/dotenv#how-do-i-use-dotenv-with-import
dotenv.config()
import { MssqlAccessorService } from './MssqlAccessorService';
import { CronService } from './cronService';



export class TwitterService {
	twitterClient: TwitterApi;
	static mssqlUser: MssqlAccessorService<User>;
	static twitterClient: any;
	static apiHelper: any;
	static user: User;
	static formattedDate: string | undefined ;

	constructor() {
		TwitterService.mssqlUser = new MssqlAccessorService<User>("/Users");
		TwitterService.mssqlUser.connect();
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

	public static async getTweets(record: TwitterRecord): Promise<any> {
		TwitterService.formattedDate = TwitterService.getDateinterval(Number(process.env.TWITTER_SEARCH_MINUTES_RANGE));
		console.log(`Searching tweets in proccess ${record.engineCronUuid} with data: 
		teamName='${record.teamName}'
		recordName='${record.eventName}'
		authorId='${record.authorId}'`);
		const result: TweetSearchRecentV2Paginator =await this.twitterClient.v2.search(record.eventName + " " + record.teamName,
						{ 'expansions': 'author_id', 'start_time': TwitterService.formattedDate })
		console.log("==============================================")
		try {
			if (TwitterService.matchTweets(result.tweets, record)) {
				TwitterService.updateTweetParticipant(record);
			}
		} catch (error) {
			console.log("smthing goes wrong.....");
		}
	}

	public static matchTweets(tweets: any, record: TwitterRecord): boolean{
		for (const tweet of tweets) {
			console.log("author_id: " + tweet.author_id);
			console.log("text: " + tweet.text.substring(0, 20));
			if (tweet.author_id == String(record.authorId)) {// get match from team members
				return true;
			}
		}
		return false;
	}

	public static updateTweetParticipant(record: TwitterRecord){
		console.log("Found a match!");
		const filter : Record<string, any> = { "Id": record.participantId }
		TwitterService.mssqlUser.getOneWithCondition(filter);
		TwitterService.user = TwitterService.mssqlUser.message;
		TwitterService.mssqlUser.updateOne(TwitterService.user.id, TwitterService.user);
		console.log(`Stopping cron job with uuid=${record.engineCronUuid}`);
		CronService.stopCron(record.engineCronUuid);
		console.log("Tweet is stopped");
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

	public static getDateinterval(minutesInterval: number): string{
		let currentDate = new Date();
		currentDate.setMinutes(currentDate.getMinutes() - minutesInterval);
		let formattedDate = currentDate.toISOString();
		return formattedDate;
	}
}
