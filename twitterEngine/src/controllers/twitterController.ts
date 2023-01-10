import { TwitterApi } from 'twitter-api-v2';
import { Request, Response, NextFunction } from 'express';
import { TwitterService } from '../services/TwitterService';


const twitterService = new TwitterService()

  const getTweets = async (req: Request, res: Response, next: NextFunction) => {
    twitterService.getTweets(req.body.query)
    .then((result: any) => {
      return res.status(200).json({
          message: result
      });
  })
};

export {getTweets}