import { TwitterApi } from 'twitter-api-v2';
import { Request, Response, NextFunction } from 'express';
import { TwitterService } from '../services/twitterService';


const twitterService = new TwitterService()

const getTweets = async (twitterClient: TwitterApi) => {

    return 0;
    //await twitterClient.v1.tweet('Hello, this is a test.');
    // rest of the code here
  };


  const getAllTweets = async (req: Request, res: Response, next: NextFunction) => {
    twitterService.getAllTweets(req.params.query)
    .then((result: any) => {
      return res.status(200).json({
          message: result
      });
  })
};

export {getAllTweets, getTweets}