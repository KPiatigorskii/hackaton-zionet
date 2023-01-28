import express from 'express';
import { getUser, setUser } from '../controllers/mssqlAccessorController';
const router = express.Router();

router.get('/getUser/:user_id', getUser);
router.post('/setUser', setUser);

export default { router };