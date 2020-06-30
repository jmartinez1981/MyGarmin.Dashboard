import * as React from "react";
import { Admin, Resource } from 'react-admin';
import { UserList, UserEdit, UserCreate } from './users/index';
import { StravaConnectionList, StravaConnectionEdit, StravaConectionCreate } from './stravaConnections/index';
import authRestProvider from './dataProviders/authRestProvider';
import Dashboard from './Dashboard';
import authProvider from './authProvider';

const App = () => (
      <Admin dashboard={Dashboard} authProvider={authProvider} dataProvider={authRestProvider}>
        <Resource name="Users" list={UserList} edit={UserEdit} create={UserCreate}/>
        <Resource label="Strava Connections" name="StravaConnections" list={StravaConnectionList} edit={StravaConnectionEdit} create={StravaConectionCreate}/>
      </Admin>
  );
export default App;
