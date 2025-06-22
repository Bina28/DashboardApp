// import axios from "axios";

// const api = axios.create({
//   baseURL: import.meta.env.VITE_API_URL,
// });

// export default api;

import axios from "axios";

const api = axios.create({
  baseURL: "/api", // всё будет автоматически проксироваться на backend
});

export default api;
