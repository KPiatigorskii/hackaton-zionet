import { TwitterApi } from 'twitter-api-v2';
import { Request, Response, NextFunction } from 'express';
import { CronService } from '../services/CronService';
import { TwitterService } from '../services/TwitterService';
import { SignalRClient } from '../services/MssqlAccessorService'


const mssqlAccessorService = new SignalRClient('http://localhost:7277/users')

const getUser = (req: Request, res: Response) => {
	console.log("starting request from signalr for user id" + req.params.user_id)
	const result = mssqlAccessorService.testFunction();
	return res.status(200).json({
		message: result
	});
};

const setUser = async (req: Request, res: Response) => {
	const result = mssqlAccessorService.testFunction();
	return res.status(200).json({
		message: result
	});
};

export { getUser, setUser }