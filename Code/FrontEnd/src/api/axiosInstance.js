// src/api/axiosInstance.js
import axios from 'axios';

const axiosInstance = axios.create({
  baseURL: 'http://localhost:5134/api', 
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add an interceptor to attach the JWT token to every request
axiosInstance.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token'); // or sessionStorage if you store it there
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

export default axiosInstance;
