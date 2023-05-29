import { createRouter, createWebHistory } from "vue-router";
import route from './routes.js';
import admin from './admin.js';

const routes = [...route, ...admin]; //Use 3 point use all in pages js

const router = createRouter({
  history: createWebHistory(), //use Webhistory
  routes,
});


export default router;