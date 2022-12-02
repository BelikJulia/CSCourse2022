var applicationConfig = {
    clientID: '[ClientId]',
    authority: "https://[TenantName].b2clogin.com/[TenantName].onmicrosoft.com/B2C_1_{YourInitials}}",
};

var clientApplication = new Msal.UserAgentApplication(applicationConfig.clientID, applicationConfig.authority, authCallback, { cacheLocation: 'localStorage', validateAuthority: false });
function authCallback(errorDesc, token, error, tokenType) {
    if (token) {
    }
    else {
        logMessage(error + ":" + errorDesc);
    }
}

var token = "";

function login() {
    clientApplication.loginPopup().then(function (idToken) {
        //Login Success
        document.getElementById("loginStatus").innerText = "Login Successfully";
        token = idToken;
    }, function (error) {
        document.getElementById("loginStatus").innerText = error;
    });
}

function request(token) {
    var headers = new Headers();
    var bearer = "Bearer " + token;
    headers.append("Authorization", bearer);
    var options = {
        method: "GET",
        headers: headers
    };

    fetch("https://localhost:5001/api/values", options)
        .then(function (response) {
            response.json().then(function (body) {
                document.getElementById("responseData").innerText = JSON.stringify(body);
            });
        });
}