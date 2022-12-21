const errorArea = $('#textError')
$('#loginButton').click(function (e) {
    e.preventDefault()
    hideError()
    showLoader()
    const login = $('#userName').val()
    const password = $('#password').val()
    const isrememberMe = $('#rememberMe').prop('checked')

    if (login.length === 0) {
        appendError('введите логин')
        return;
    }

    if (password.length === 0) {
        appendError('введите пароль')
        return;
    }
    
    const antiforgeyToken = $("#UserLoginModal input[name='__RequestVerificationToken']").val()
    let data = {
        __RequestVerificationToken: antiforgeyToken,
        login: login,
        password: password,
        rememberMe: isrememberMe
    };
    $.post({
        url: '/Account/Login',
        data: data,
        success: function (result) {
            if (result.includes('Неверный')) {
                hideLoader()
                appendError(result)
            } else if (result.length > 30) {
                document.write(result)
            } else {
                location.reload()
            }
        },
        error: function (e) {
            hideLoader()
            console.log(e)
        }
    })
})

function appendError(error) {
    errorArea.show()
    errorArea.html(error)
}

function hideError() {
    errorArea.hide()
}

function showLoader(){
    $('.loader').show()
    $('#loginButton').hide()
}

function hideLoader(){
    $('.loader').hide()
    $('#loginButton').show()
}
