// src/api/axiosInstance.js
import axios from 'axios';

const instance = axios.create({
  baseURL: 'https://your-backend-url.com/api', // ✅ change when backend is ready
  headers: {
    'Content-Type': 'application/json',
  },
});

export default instance;
