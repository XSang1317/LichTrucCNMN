import 'bootstrap/dist/css/bootstrap.css'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import axios from 'axios'
window.axios = axios;
////////////////////////////////////////////////////////////////
import { createPinia } from 'pinia'
import "./style.css"
//import "./../dist/output.css"
import Antd from 'ant-design-vue';
import "ant-design-vue/dist/antd.css"
import "bootstrap/dist/css/bootstrap-grid.min.css"
import "./static/fontawesome-free-6.3.0-web/css/all.min.css"
import '@progress/kendo-theme-default/dist/all.css';
import { Scheduler } from '@progress/kendo-vue-scheduler'

import 'bootstrap/dist/css/bootstrap.css'

/* import BootstrapVue from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap-vue/dist/bootstrap-vue.css' */

import '../node_modules/nprogress/nprogress.css' 

const app = createApp(App)
app.use(createPinia())
//app.use(BootstrapVue)
app.use(router)
app.use(Antd)
app.use(Scheduler);
app.mount('#app')
