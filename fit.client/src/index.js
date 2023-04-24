import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import axios from 'axios';

axios.defaults.baseURL = process.env.NODE_ENV === 'production' ? "/api" : "https://localhost:5001/api";
console.log(process.env.NODE_ENV)
//"'%NODE_ENV%')
//const BASE_URL = process.env.NODE_ENV === 'production' ? "/api" : "https://localhost:5001/api";
const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
