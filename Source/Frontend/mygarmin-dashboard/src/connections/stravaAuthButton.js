import * as React from "react";
import { Button } from 'react-admin';

const StravaAuthButton = ({ record }) => {
    const isButtonVisible = record.connectionType === 'Strava';
    
    const stravaAuth = (e) =>{
        e.stopPropagation();
        
        let basePath = 'https://www.strava.com/oauth/authorize?';
        let parameters = 'client_id=' + record.id
        + '&redirect_uri=http://localhost:3000/stravaAuthCallback'
        + '&response_type=code'
        + '&approval_prompt=auto'
        + '&scope=activity:read_all';
        
        localStorage.setItem('clientId', record.id);

        window.location = basePath+parameters;
        //window.location.replace(basePath+parameters);
    }

    if (isButtonVisible){
        return <Button label="Authentication" onClick={stravaAuth} />;
    }
    return null;    
};

export default StravaAuthButton;