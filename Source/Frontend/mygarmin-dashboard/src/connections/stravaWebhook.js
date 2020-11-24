import { useEffect } from 'react';
import { useDataProvider, useNotify, useRedirect } from 'react-admin';
import queryString from 'querystring';
import Configuration from '../configuration/config';

function StravaWebhook(props) {
    const notify = useNotify();
    const redirect = useRedirect();
    const dataProvider = useDataProvider();

    useEffect(() => {        
        const clientId = localStorage.getItem('clientId');
        const challenge = getChallenge(redirect, notify);

        if (challenge === null || !isVerifyTokenValid(redirect, notify, Configuration.stravaApi.VERIFY_TOKEN)){
            return;
        }

        dataProvider
        .create('connections/validateSubscription', { id: clientId, data: 
        { 
            challenge: challenge
        } })
        .then(response => {
        })
        .catch(error => {
            // failure side effects go here 
            console.log(`error validating subscription: ${error.message}`, 'error');
        });   
    });  

    return null;
}

function isVerifyTokenValid(redirect, notify, verifyToken){
    const params = queryString.parse(window.location.search);

    if (params.error !== undefined){
        console.log(`trying to subscribe. Error: ${params.error}`, 'error');
        return false;
    }

    if (params.hub.verify_token !== verifyToken){
        console.log(`trying to subscribe. Error: Token invalid`, 'error');
        return false;
    }
    return true;
}

function getChallenge(redirect, notify){
    const params = queryString.parse(window.location.search);

    if (params.error !== undefined){
        console.log(`trying to subscribe. Error: ${params.error}`, 'error');
        return null;
    }

    return params.hub.challenge;
}

export default StravaWebhook;


