import express from 'express';
import { startCron, stopCron } from '../controllers/cronContoller';
const router = express.Router();

//router.get('/get',middleware.verifyToken, middleware.verifyToken([Role.Administrator, Role.RegularUser]), controller.getAllCategories);
router.post('/start', startCron);
router.post('/stop', stopCron);

export default { router };