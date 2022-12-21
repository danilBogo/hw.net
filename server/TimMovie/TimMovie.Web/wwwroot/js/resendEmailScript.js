function resendEmail(){
    $('.loader').toggleClass('hide');
    setTimeout(()=> {
        $('.loader').toggleClass('hide');
    }, 1500);
    console.log('clicked');
    let email = $("#emailInput").val();
    $.ajax({
        type: "POST",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        url:"/Account/SendConfirmEmail",
        data:{email:email},
        success: function (){
            console.log('resend email');
        },
        error: function () {
            $('.loader').toggleClass('hide');
            console.log('error occured')
        }
    })
}