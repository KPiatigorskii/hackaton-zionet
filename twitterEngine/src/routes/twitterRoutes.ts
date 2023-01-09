import express from 'express';
import {getAllTweets, getTweets} from '../controllers/twitterController';
const router = express.Router();

//router.get('/get',middleware.verifyToken, middleware.verifyToken([Role.Administrator, Role.RegularUser]), controller.getAllCategories);
router.get('/getTweets', getAllTweets)

export default { router };