// src/api/axiosInstance.js
import axios from 'axios';

const axiosInstance = axios.create({
  baseURL: 'https://your-backend.com/api', 
  headers: {
    'Content-Type': 'application/json',
  },
});

export default axiosInstance;
