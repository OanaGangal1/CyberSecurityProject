"use strict"

const authController = function ()
{
    const context =
    {
        URLS:
        {
            registerUrl: "Register/Post",
            loginUrl: "Login/Post"
        },
        registerBtn: $("#registerBtn"),
        registerForm: $("#registerForm"),
        loginBtn: $("#loginBtn"),
        loginForm: $("#loginForm"),
        errorMessageContainer: $("#errorMessageContainer")
    };

    const onRegister = function ()
    {
        utilities.blockUI();
        let reqData = utilities.formDataAsJson(context.registerForm);

        $.post(context.URLS.registerUrl, reqData)
            .done(function (data) {
                console.log(data);
                toastr.success(data.responseText);
                utilities.unblockUI();
            })
            .fail(function (data) {
                toastr.error(data.responseText);
                utilities.unblockUI();
            });
    };

    const onLogin = function ()
    {
        let reqData = utilities.formDataAsJson(context.loginForm);
        $.post(context.URLS.loginUrl, reqData)
            .done(function (data) {
                window.localStorage.setItem("AccessToken", data);
            })
            .fail(function (data) {
                toastr.error(data.responseText);
            });
    };

    const initAuth = function ()
    {
        context.registerBtn.on('click', onRegister);
        context.loginBtn.on('click', onLogin);
    };

    return {
        init: initAuth
    }
}();



$(document).ready(function () {
    authController.init();
});