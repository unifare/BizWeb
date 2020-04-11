/// <reference path="javascripts/oidc-client.js" />

 

//document.getElementById("login").addEventListener("click", login, false);
//document.getElementById("api").addEventListener("click", api, false);
//document.getElementById("logout").addEventListener("click", logout, false);

//var config = {
//    authority: "http://oauth.66wave.com",
//    client_id: "post_ed",
//    client_secret:"uninoteapisdkfsldfsdfber232g4gnip",
//    redirect_uri: "http://localhost:6008/public/callback.html",
//    response_type: "id_token token",
//    scope:"UniNote_WebApi openid offline_access",
//    post_logout_redirect_uri: "http://localhost:6008",
//};

var config = {
    authority: "http://oauth.66wave.com",
    client_id: "post_ed",
    client_secret: "uninoteapisdkfsldfsdfber232g4gnip",
    redirect_uri: "http://localhost:6008/public/callback.html",
    response_type: "id_token token",
    scope: "UniNote_WebApi UniNote_localApi api1 openid profile offline_access",
    post_logout_redirect_uri: "http://localhost:6008",
};

var mgr = new Oidc.UserManager(config);

mgr.getUser().then(function (user) {
    if (user) {
        console.log("User logged in", user.profile);
    }
    else {
        console. log("User not logged in");
    }
});

function login() {
    mgr.signinRedirect();
}

function api() {
    mgr.getUser().then(function (user) {
        var url = "http://uninote.66wave.com/api/values";

        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
            console.log(  JSON.parse(xhr.responseText));
        }
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
}

function logout() {
    mgr.signoutRedirect();
}