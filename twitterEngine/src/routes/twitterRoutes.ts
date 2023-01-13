import express from 'express';
import { getTweets, sendTweets } from '../controllers/twitterController';
const router = express.Router();

//router.get('/get',middleware.verifyToken, middleware.verifyToken([Role.Administrator, Role.RegularUser]), controller.getAllCategories);
router.get('/getTweets', getTweets)
router.post('/sendTweet', sendTweets)
export default { router };