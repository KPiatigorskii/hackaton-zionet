import express from 'express';
import { startCron, stopCron, getCronByUuid } from '../controllers/cronContoller';
const router = express.Router();

router.post('/start', startCron);
router.post('/stop', stopCron);
router.get('/:cronUuid/status', getCronByUuid)

export default { router };