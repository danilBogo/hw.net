$(function () {
    let currentRequest = null;
    let element = $('#input-search-entities-navbar');

    $('input').on('keyup', function () {
        let elm = $(this);
        let time = (new Date()).getTime();
        let delay = 150;

        elm.attr({'keyup': time});
        elm.off('keydown');
        elm.off('keypress');
        elm.on('keydown', function (e) {
            $(this).attr({'keyup': time});
        });
        elm.on('keypress', function (e) {
            $(this).attr({'keyup': time});
        });

        setTimeout(function () {
            let oldtime = parseFloat(elm.attr('keyup'));
            if (oldtime <= (new Date()).getTime() - delay & oldtime > 0 &&
                elm.attr('keyup') !== '' &&
                typeof elm.attr('keyup') !== 'undefined') {
                currentRequest?.abort();
                SendRequest();
                elm.removeAttr('keyup');
            }
        }, delay);
    });

    function SendRequest() {
        if (element.val().trim() === '') {
            $('.search-elements').html("");
            return;
        }
        currentRequest = $.post({
            url: "/Search/SearchEntityResults",
            data: {namePart: element.val()},
            success: function (data) {
                if ($('#input-search-entities-navbar').val().trim() === '')
                    $('.search-elements').html("");
                else
                    $('.search-elements').html(data);
            }
        });
    }

    $("#buttonClose").click(function () {
        $('#dialogContent').html();
    });
})