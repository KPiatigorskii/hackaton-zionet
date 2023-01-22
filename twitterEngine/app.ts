import http from 'http';
import express, { Express } from 'express';

import  twitterRoutes from '../twitterEngine/src/routes/twitterRoutes'
import cronRoutes from '../twitterEngine/src/routes/cronRoutes'
import mssqlAccessorRoutes from './src/routes/mssqlAccessorRoutes';
import { MssqlAccessorService } from "./src/services/MssqlAccessorService"
import { TwitterService } from "./src/services/TwitterService"

import * as dotenv from 'dotenv' // see https://github.com/motdotla/dotenv#how-do-i-use-dotenv-with-import
import { startCron } from './src/controllers/cronContoller';
import { CronService } from './src/services/CronService';
import { TwitterRecord } from './src/entities';
dotenv.config()

// Instantiate with desired auth type (here's Bearer v2 auth)

const expressSanitizer = require("express-sanitizer");

// Importing the fs and https modules -------------- STEP 1
const https = require("https");

const fs = require('fs'); // TODO: f

// Read the certificate and the private key for the https server options
// ------------------- STEP 2

const httpsOptions = {
  key: fs.readFileSync('./security/localhost.key'),
  cert: fs.readFileSync('./security/localhost.crt')
}

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
app.use((req, res, next) => {
    const error = new Error('not found');
    return res.status(404).json({
        message: error.message
    });
});

app.get('/', (req, res) => {
  res.send("IT'S WORKING!")
})

const PORT: any = process.env.PORT ?? 6978;

// app.listen(PORT, () => {
//     console.log(`Server started on port ${PORT}`);
//   });
  
  // Create the https server by initializing it with 'options'
  // -------------------- STEP 3
  http.createServer(app).listen(PORT, () => {
    console.log(`HTTP server started on port ${PORT}`);
  });

  import axios from 'axios';
  //const httpsAgent = new https.Agent({ rejectUnauthorized: false });
  axios.get('http://localhost:5192/api/TwitterRecords')
    .then(response => {
      // Use the data here in your app.ts file
      const data = response.data;
      console.log("HOOORAY");
    })
    .catch(error => {
      console.log(error);
    });












// let mssql = new MssqlAccessorService("https://localhost:7277/");
// let twitterService = new TwitterService();

// let twitterRecords: TwitterRecord[] = mssql.getTwitterRecords();
// let twitterSortedRecords: TwitterRecord[] = twitterService.getRecordsWithInvalidInstances(twitterRecords);
// twitterSortedRecords.forEach((record: TwitterRecord) => {
//   CronService.startCron(record, twitterService,"");
// });

