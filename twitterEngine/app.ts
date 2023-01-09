import http from 'http';
import express, { Express } from 'express';
import { TwitterApi } from 'twitter-api-v2';

// Instantiate with desired auth type (here's Bearer v2 auth)


const twitterClient = new TwitterApi({
  appKey: 'M4TuwEsziKK5gZS1uGujpsuHM',
  appSecret: 'i4WkYEPn27YUohbfPRsi3DStkyFV45rHTYC4M13Rp0i5XLXNYV',
  accessToken: '1606734720569692160-IV0JnoYdvyARYLXzXC1qv89uy36dT7',
  accessSecret: 'kw6RrolXZrrebqX54GLSdeCzjGrsk2coU7vBQ3toYCjyh',
});


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