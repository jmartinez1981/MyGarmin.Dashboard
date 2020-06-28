import * as React from "react";
import { fetchUtils, Admin, Resource } from 'react-admin';
import { UserList, UserEdit, UserCreate } from './users';
import simpleRestProvider from 'ra-data-simple-rest';
import Dashboard from './Dashboard';
import authProvider from './authProvider';

const httpClient = (url, options = {}) => {
  if (!options.headers) {
      options.headers = new Headers({ Accept: 'application/json' });
  }
  const token = localStorage.getItem('token');
  options.headers.set('Authorization', `Bearer ${token}`);
  options.mode = 'cors';
  return fetchUtils.fetchJson(url, options);
};

const dataProvider = simpleRestProvider('http://localhost:5874/api', httpClient);

const App = () => (
      <Admin dashboard={Dashboard} authProvider={authProvider} dataProvider={dataProvider}>
        <Resource name="users" list={UserList} edit={UserEdit} create={UserCreate}/>
      </Admin>
  );
export default App;
