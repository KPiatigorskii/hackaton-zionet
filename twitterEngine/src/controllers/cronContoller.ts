import { TwitterApi } from 'twitter-api-v2';
import { Request, Response, NextFunction } from 'express';
import { CronService } from '../services/CronService';
import { TwitterService } from '../services/TwitterService';


const cronService = new CronService()
const twitterService = new TwitterService()

  const startCron = (req: Request, res: Response) => {
    console.log("starting cron job with query " + req.body.query)
    const result = cronService.startCron(twitterService, req.body.query, req.body.team_id);
    return res.status(200).json({
        message: result
    });
};

const stopCron = async (req: Request, res: Response) => {
  const result = cronService.stopCron(req.body.team_id);
    return res.status(200).json({
        message: result
    });
};

export { startCron, stopCron }