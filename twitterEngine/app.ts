import http from 'http';
import express, { Express } from 'express';

import  twitterRoutes from '../twitterEngine/src/routes/twitterRoutes'
import cronRoutes from '../twitterEngine/src/routes/cronRoutes'
import mssqlAccessorRoutes from './src/routes/mssqlAccessorRoutes';

import * as dotenv from 'dotenv'
import { InitializeController } from './src/controllers/initializeController';
import { TwitterRecord } from './src/entities';
dotenv.config()

// Initialize instance of express
const app = express();

// Init Middleware
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
/** Parse the request */
app.use(express.urlencoded({ extended: false }));
/** Takes care of JSON data */
app.use(express.json());
/** RULES OF OUR API */
app.use((req, res, next) => {
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
app.use('/CronSchedule', cronRoutes.router);
app.use('/twitter', twitterRoutes.router);
app.use('/mssqlAccessor',mssqlAccessorRoutes.router )

/** Error handling */
// app.use((req, res, next) => {
//     const error = new Error('not found');
//     return res.status(404).json({
//         message: error.message
//     });
// });

app.use((req, res, next) => {
  console.log('Time:', Date.now())
})


//InitializeController.getAllActualRecords();

export const PORT: any = process.env.PORT ?? 6978;


http.createServer(app).listen(PORT, () => {
  console.log(`HTTP server started on port ${PORT}`); 
});


