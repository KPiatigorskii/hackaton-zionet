import { TwitterApi } from 'twitter-api-v2';
import { Request, Response, NextFunction } from 'express';
import { CronService } from '../services/CronService';
import { TwitterService } from '../services/TwitterService';
import { MssqlAccessorService } from '../services/MssqlAccessorService'
import { SignalRHelper } from '../helpers/SignalRHelper';
import { User } from '../entities';

	
const connectionUrl = "http://localhost:5192/Users";
const accessToken = "";


const mssqlAccessorService = new MssqlAccessorService("http://localhost:5192/Users")

const getUser = (req: Request, res: Response) => {
	mssqlAccessorService.connect( String(req.headers['authorization']) || '')
	const result = mssqlAccessorService.getUser(Number(req.params.user_id))
	.then((result: User) => {
            return res.status(200).json(result);
        })
        .catch((error: any) => {
            return "error";
        });
};

const setUser = async (req: Request, res: Response): Promise<any> => {
	await mssqlAccessorService.connect( String(req.headers['authorization']) || '')
	const result =await  mssqlAccessorService.setUser(Number(req.body.id), req.body as User )
	return res.status(200).json({
		message: result
	});
};

export { getUser, setUser }