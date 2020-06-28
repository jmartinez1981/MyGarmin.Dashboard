export default {
    // called when the user attempts to log in
    login: async ({ username, password }) =>  {
        const request = new Request('http://localhost:5874/api/auth/authenticate', {
            method: 'POST',
            body: JSON.stringify({ 'username': username, 'password' : password }),
            mode: 'cors',
            headers: new Headers({ 'Content-Type': 'application/json',  }),
        });
        const response = await fetch(request);
        if (response.status < 200 || response.status >= 300) {
            throw new Error(response.statusText);
        }
        const authenticateResponse = await response.json();
        localStorage.setItem('token', authenticateResponse.token);
        localStorage.setItem('username', authenticateResponse.username);
        return Promise.resolve();
    },
    // called when the user clicks on the logout button
    logout: () => {
        localStorage.removeItem('username');
        localStorage.removeItem('token');
        return Promise.resolve();
    },
    // called when the API returns an error
    checkError: ({ status }) => {
        if (status === 401 || status === 403) {
            localStorage.removeItem('username');
            return Promise.reject();
        }
        return Promise.resolve();
    },
    // called when the user navigates to a new location, to check for authentication
    checkAuth: () => {
        return localStorage.getItem('username')
            ? Promise.resolve()
            : Promise.reject();
    },
    // called when the user navigates to a new location, to check for permissions / roles
    getPermissions: () => Promise.resolve(),
};