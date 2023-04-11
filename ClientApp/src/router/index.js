import { createRouter, createWebHistory } from "vue-router";
import route from './routes.js';
import admin from './admin.js';


/* import NProgress from "nprogress";
import "nprogress/nprogress.css"

import store from "../stores";
 */
const routes = [...route, ...admin]; //Use 3 point use all in pages js

const router = createRouter({
  history: createWebHistory(), //use Webhistory
  routes,
});

/* router.beforeEach((to, from, next) => {
  NProgress.start();
  if (to.matched.some((route) => route.meta.noAuth)) {
    next();
  } else if (!store.getters.isAuthenticated) {
    next({ path: "/login", query: { redirect: to.path } });
  } else if (to.meta.permission && to.meta.permission.length) {
    if (!store.getters.hasPermission(to.meta.permission))
      next({ path: "/" });
    else next();
  } else {
    next();
  }
});

router.afterEach(() => {
  NProgress.done();
}); */

export default router;