import * as React from "react";
import { useMutation, useNotify, useRedirect, Button } from 'react-admin';
import { useHistory } from "react-router-dom";
import { UserManager } from 'oidc-client';

const StravaAuthButton = ({ record }) => {
    const isButtonVisible = record.connectionType === 'Strava';
    const notify = useNotify();
    const redirect = useRedirect();
    const history = useHistory();
    const stravaAuth = (e) =>{
        let basePath = 'https://www.strava.com/oauth/authorize?';
        let queryString = encodeURIComponent('client_id=50135' +
        + '&redirectUri=http://www.google.es'
        + '&response_type=code'
        + '&approval_prompt=auto'
        + '&scope=activity:read');
        const querystring = require('querystring');

        const targetUrl = 'https://www.strava.com/oauth/authorize?' + querystring.stringify({
            client_id: 50135,
            redirect_uri: 'http://localhost:3000/#/callback',
            response_type: 'code',
            scope: 'activity:read'
          });
        e.preventDefault();

        window.open(targetUrl, "_blank") //to open new page
        /*const issuer = 'https://www.strava.com/oauth/authorize';
        const clientId = 50135;
        const redirectUrl = 'http://localhost:3000/#/Connections';

        const userManager = new UserManager({
            authority: issuer,
            client_id: clientId,
            redirect_uri: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ''}/callback`,
            response_type: 'code',
            scope: 'activity:read_all'
        });

        userManager.signinRedirect()
            .catch((error) => {
                console.log(error);
            });

        //userManager.signinRedirect();*/
    }


    /*const [stravaAuth, { loading }] = useMutation(
        {
            type: 'update',
            resource: 'connections/stravaAuthentication',
            payload: { id: record.id, data: { } },
        },
        {
            onSuccess: ({ data }) => {
                redirect('/connections');
                notify('Strava authentication done successfully');
            },
            onFailure: (error) => notify(`Strava authentication failed with error: ${error.message}`, 'warning'),
        }
    );*/
    if (isButtonVisible){
        return <Button label="Authentication" onClick={stravaAuth} />;
    }
    return null;    
};

export default StravaAuthButton;