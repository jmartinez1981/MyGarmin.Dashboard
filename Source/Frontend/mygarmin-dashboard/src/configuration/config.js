const dev = {
    api: {
        HOSTNAME: "http://localhost:5874/"
    }
};

const prod = {
    api: {
        HOSTNAME: "http://myTrainingDashboard/"
    }
};

const config = process.env.REACT_APP_STAGE === 'production'
    ? prod
    : dev;

export default {
    api: {
        AUTH_ENDPOINT: "auth/authenticate",
    },
    stravaApi: {
        HOSTNAME: "https://www.strava.com/oauth/",
        CALLBACK_ENDPOINT: "http://localhost:3000/stravaAuthCallback",
        CALLBACK_WEBHOOK: "http://localhost:3000/webhook",
        VERIFY_TOKEN: "STRAVA_TOKEN",
        AUTH_ENDPOINT: "authorize?client_id=[CLIENT_ID]&redirect_uri=[REDIRECT_URI]&response_type=code&approval_prompt=auto&scope=profile:read_all,activity:read_all",
        TOKEN_ENDPOINT: "token?client_id=[CLIENT_ID]&client_secret=[CLIENT_SECRET]&code=[CODE]&grant_type=authorization_code"
    },
    ...config
};
