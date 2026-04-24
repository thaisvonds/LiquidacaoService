import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5056/api' 
});

export default api;