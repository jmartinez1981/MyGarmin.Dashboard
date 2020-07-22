import * as React from "react";
import { Admin, Resource } from 'react-admin';
import { Route } from 'react-router-dom';
import createHistory from 'history/createBrowserHistory';
import { UserList, UserEdit, UserCreate } from './users/index';
import { ConnectionList } from './connections/index';
import { ConnectionCreate } from './connections/create';
import { ConnectionEdit } from './connections/edit';
import authRestProvider from './dataProviders/authRestProvider';
import Dashboard from './dashboard/DashboardPage';
import authProvider from './dataProviders/authProvider';
import stravaAuthCallback from './connections/stravaAuthCallback';
import cubejs from "@cubejs-client/core";
import { CubeProvider } from "@cubejs-client/react";
import WebSocketTransport from "@cubejs-client/ws-transport";

//const CUBEJS_API_URL = "http://localhost:4000";
const CUBEJS_TOKEN =
  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpYXQiOjE1OTUzMzk1NzYsImV4cCI6MTU5NTQyNTk3Nn0.qTEttggbcbXSa_N_qNB19Yj82NJPodeJGfZXvwizCW8";
/*const cubejsApi = cubejs(CUBEJS_TOKEN, {
  apiUrl: `${CUBEJS_API_URL}/cubejs-api/v1`
});*/

/*const cubejsApi = cubejs({
  transport: new WebSocketTransport({
    authorization: CUBEJS_TOKEN,
    apiUrl: CUBEJS_API_URL
  })
});*/

const cubejsApi = cubejs({
  transport: new WebSocketTransport({
    authorization: CUBEJS_TOKEN,
    apiUrl: 'ws://localhost:4000/'
  })
});

const history = createHistory();

const App = () => (
  <CubeProvider cubejsApi={cubejsApi}>
    <Admin 
      history={history}
      dashboard={Dashboard} 
      authProvider={authProvider} 
      dataProvider={authRestProvider} 
      customRoutes={[
        <Route
            path="/stravaAuthCallback"
            component={stravaAuthCallback}
        />,
        ]}
        >       
        <Resource name="Users" list={UserList} edit={UserEdit} create={UserCreate}/>
        <Resource name="Connections" list={ConnectionList} edit={ConnectionEdit} create={ConnectionCreate}/>
      </Admin>
  </CubeProvider>
      
  );

export default App;
