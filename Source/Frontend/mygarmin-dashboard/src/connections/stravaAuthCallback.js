import React, { useEffect } from 'react';
import { useDataProvider, useNotify, useRedirect } from 'react-admin';
import queryString from 'querystring';
import StyledLoader from '../css/styledLoader'

function StravaAuthCallback(props) {
    const notify = useNotify();
    const redirect = useRedirect();
    const dataProvider = useDataProvider();

    useEffect(() => {
        const clientId = localStorage.getItem('clientId');
        const code = getCode(redirect, notify);

        localStorage.removeItem('clientId');

        dataProvider
        .update('connections/loadData', { id: clientId, data: 
        { 
            type: "Strava",
            code: code
        } })
        .then(response => {
            // success side effects go here
            redirect('/connections');
            notify('data imported successfully');
        })
        .catch(error => {
            // failure side effects go here 
            redirect('/connections');
            notify(`error importing data: ${error.message}`, 'error');
        });   
    }, []);  

    return(
        <StyledLoader
        active='true'
            classNamePrefix='MyLoader_'
            spinner
            text='Importing Strava data...'>
        </StyledLoader>
    )      
}

function getCode(redirect, notify){
    const params = queryString.parse(window.location.search);

    if (params.error !== undefined){
        redirect('/connections');
        notify(`authorization error: ${params.error}`, 'error');
    }

    return params.code;
}
export default StravaAuthCallback;


