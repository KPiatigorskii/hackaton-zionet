import { TwitterApi } from 'twitter-api-v2';
import { Request, Response, NextFunction } from 'express';
import { TwitterService } from '../services/TwitterService';
import { CronService } from '../services/CronService';
import { Team } from '../entities';
import { TwitterRecord } from './../entities';

const twitterService = new TwitterService()

const startCron = (req: Request, res: Response, next: NextFunction) => {
	console.log("starting cron job with query " + req.body.eventName + " " + req.body.teamName)
	const result = CronService.startCron(req.body as TwitterRecord, String(req.headers['authorization']) || '');
	return res.status(200).json({
		message: result
	});
};

const stopCron = async (req: Request, res: Response, next: NextFunction) => {
	const result = CronService.stopCron(req.body.team_id, String(req.headers['authorization']) || '');
	return res.status(200).json({
		message: result
	});
};

const getCronByUuid = async (req: Request, res: Response, next: NextFunction) => {
	const result = CronService.getCronByUuid(req.params.cronUuid, String(req.headers['authorization']) || '');
	return res.status(200).json({
		message: result
	});
};

export { startCron, stopCron, getCronByUuid }