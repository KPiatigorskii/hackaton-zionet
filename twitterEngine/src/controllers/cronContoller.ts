import { TwitterApi } from 'twitter-api-v2';
import { Request, Response, NextFunction } from 'express';
import { TwitterService } from '../Services/TwitterService';
import { CronService } from '../Services/CronService';
import { Team } from '../entities';

const twitterService = new TwitterService()

  const startCron = (req: Request, res: Response) => {
    console.log("starting cron job with query " + req.body.eventName + " " + req.body.teamName)
    const result = CronService.startCron(twitterService, req.body.eventName, req.body.teamName, req.body.userTwitterId);
    return res.status(200).json({
        message: result
    });
};

const stopCron = async (req: Request, res: Response) => {
  const result = CronService.stopCron(req.body.team_id);
    return res.status(200).json({
        message: result
    });
};

export { startCron, stopCron }