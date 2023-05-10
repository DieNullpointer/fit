import { createApp } from 'vue'
import axios from "axios";
import PrimeVue from 'primevue/config';
import App from './App.vue'
import router from './router'
import store from './store.js'
import process from 'node:process'
import './assets/main.css'

axios.defaults.baseURL = process.env.NODE_ENV == 'production' ? "/api" : "https://localhost:5001/api";
// Use ASP cookie for devserver requests.
axios.defaults.withCredentials = true;

axios.get("../account/accountinfo")
  .then(response => {
    const userdata = response.data;
    store.commit("authenticate", userdata);
    const app = createApp(App);
    app.use(PrimeVue);
    app.use(router);
    app.use(store);
    app.mount('#app');
  })
  .catch(() => {
    alert("Not authenticated. Please login in ASP.NET Backend on https://localhost:5001/admin and reload.");
  });


