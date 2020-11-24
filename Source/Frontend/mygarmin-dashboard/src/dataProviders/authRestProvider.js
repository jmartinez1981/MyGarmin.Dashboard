import { fetchUtils } from 'react-admin';
import simpleRestProvider from 'ra-data-simple-rest';

const httpClient = (url, options = {}) => {
    if (!options.headers) {
        options.headers = new Headers({ Accept: 'application/json' });
    }
    const token = localStorage.getItem('token');
    options.headers.set('Authorization', `Bearer ${token}`);
    options.mode = 'cors';
    return fetchUtils.fetchJson(url, options);
  };

  const authRestProvider = simpleRestProvider('http://localhost:5874/api', httpClient);

  export default authRestProvider;
