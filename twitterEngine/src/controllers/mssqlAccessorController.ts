import { TwitterApi } from 'twitter-api-v2';
import { Request, Response, NextFunction } from 'express';
import { CronService } from '../services/CronService';
import { TwitterService } from '../services/TwitterService';
import { MssqlAccessorService } from '../services/MssqlAccessorService'
import { SignalRHelper } from '../helpers/SignalRHelper';

	
const mssqlAccessorService = new MssqlAccessorService('http://localhost:7277/users')

const getUser = (req: Request, res: Response) => {
	mssqlAccessorService.connect( String(req.headers['authorization']) || '')
	const result = mssqlAccessorService.getUser(Number(req.params.user_id))
	.then((result: any) => {
            return res.status(200).json(result);
        })
        .catch((error: any) => {
            return "error";
        });
};

const setUser = async (req: Request, res: Response) => {
	//const result = mssqlAccessorService.testFunction();
	return res.status(200).json({
		message: ""
	});
};

export { getUser, setUser }