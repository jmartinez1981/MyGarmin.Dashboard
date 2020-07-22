import * as React from "react";
import { useRedirect, Button } from 'react-admin';
import Configuration from '../configuration/config';

const ImportDataButton = ({ record }) => {
    
    const loadData = (e) =>{

        e.stopPropagation();

        switch(record.connectionType){
            case 'Strava':
                StravaLoadData(record.id);
                return;
                
            case 'Garmin':
                return;
            default:
                return;
        }
    }
    
    function StravaLoadData(clientId){
        var token = GetToken(clientId);
    
        if (token === null){
            DoStravaAuth(clientId);
        }
    }

    function DoStravaAuth(clientId){
        let url = Configuration.stravaApi.HOSTNAME + Configuration.stravaApi.AUTH_ENDPOINT;
        url = url.replace("[CLIENT_ID]", clientId).replace("[REDIRECT_URI]", Configuration.stravaApi.CALLBACK_ENDPOINT);
        
        localStorage.setItem('clientId', record.id);

        window.location = url;
    }

    function GetToken(clientId){
        return null;
    }
    
    return <Button label="Import Data" onClick={loadData} />;

};

export default ImportDataButton;