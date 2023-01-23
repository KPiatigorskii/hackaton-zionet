import { TwitterApi } from 'twitter-api-v2';
import { Request, Response, NextFunction } from 'express';
import { TwitterService } from '../services/TwitterService';
import { TeamMember, Team } from '../entities';

const twitterService = new TwitterService()

const getTweets = async (req: Request, res: Response, next: NextFunction) => {
	TwitterService.getTweets(req.body.eventName, req.body.teamName, req.body.userId, req.body.userTwitterId, String(req.headers['authorization']) || '')
		.then((result: any) => {
			return res.status(200).json({
				message: result
			});
		})
};

const sendTweets = async (req: Request, res: Response, next: NextFunction) => {
	twitterService.sendTweet(req.body.tweetString)
		.then((result: any) => {
			return res.status(200).json({
				message: result
			});
		})
};

export { getTweets, sendTweets }