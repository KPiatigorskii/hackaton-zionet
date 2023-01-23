import { TwitterApi } from 'twitter-api-v2';
import { Request, Response, NextFunction } from 'express';
import { CronService } from '../services/CronService';
import { TwitterService } from '../services/TwitterService';
import { MssqlAccessorService } from '../services/MssqlAccessorService'
import { SignalRHelper } from '../helpers/SignalRHelper';
import { User } from '../entities';

	

const getUser = (req: Request, res: Response) => {
	const mssqlAccessorService = new MssqlAccessorService<User>("http://localhost:5192/Users")
	mssqlAccessorService.connect( String(req.headers['authorization']) || '')
	const result = mssqlAccessorService.getOne(Number(req.params.user_id))
	.then((result: User) => {
            return res.status(200).json(result);
        })
        .catch((error: any) => {
            return "error";
        });
};

const setUser = async (req: Request, res: Response): Promise<any> => {
	const mssqlAccessorService = new MssqlAccessorService<User>("http://localhost:5192/Users")
	await mssqlAccessorService.connect( String(req.headers['authorization']) || '')
	const result = await  mssqlAccessorService.updateOne(Number(req.body.id), req.body as User )
	return res.status(200).json({
		message: result
	});
};



export { getUser, setUser }


// checkJwt
