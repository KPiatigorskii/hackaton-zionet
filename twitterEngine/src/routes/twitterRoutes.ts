import express from 'express';
import {getTweets} from '../controllers/twitterController';
const router = express.Router();

//router.get('/get',middleware.verifyToken, middleware.verifyToken([Role.Administrator, Role.RegularUser]), controller.getAllCategories);
router.get('/getTweets', getTweets)

export default { router };