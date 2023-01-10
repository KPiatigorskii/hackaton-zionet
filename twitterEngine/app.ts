import http from 'http';
import express, { Express } from 'express';

import  twitterRoutes from '../twitterEngine/src/routes/twitterRoutes'
import cronRoutes from '../twitterEngine/src/routes/cronRoutes'
import mssqlAccessorRoutes from './src/routes/mssqlAccessorRoutes';

import { TwitterService } from '../twitterEngine/src/services/TwitterService';

import * as dotenv from 'dotenv' // see https://github.com/motdotla/dotenv#how-do-i-use-dotenv-with-import
dotenv.config()



// Instantiate with desired auth type (here's Bearer v2 auth)




const router: Express = express();

/** Parse the request */
router.use(express.urlencoded({ extended: false }));
/** Takes care of JSON data */
router.use(express.json());

/** RULES OF OUR API */
router.use((req, res, next) => {
    // set the CORS policy
    res.header('Access-Control-Allow-Origin', '*');
    // set the CORS headers
    res.header('Access-Control-Allow-Headers', 'origin, X-Requested-With,Content-Type,Accept, Authorization');
    // set the CORS method headers
    if (req.method === 'OPTIONS') {
        res.header('Access-Control-Allow-Methods', 'GET PATCH DELETE POST');
        return res.status(200).json({});
    }
    next();
});

// /** Routes */
router.use('/CronSchedule', cronRoutes.router);
router.use('/twitter/', twitterRoutes.router);
router.use('/mssqlAccessor/',mssqlAccessorRoutes.router )

/** Error handling */
router.use((req, res, next) => {
    const error = new Error('not found');
    return res.status(404).json({
        message: error.message
    });
});

   /** Server */
const httpServer = http.createServer(router); 
const PORT: any = process.env.PORT ?? 6978;
httpServer.listen(
    PORT, 
    () => {
        console.log(`The server is running on port ${PORT}`)
    });

