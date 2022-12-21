const durationForShow = 65;
const nameAdditionalInformationClassName = ".film-card__more-info-container";

function displayMoreInfo() {
    $(this)
        .find(nameAdditionalInformationClassName)
        .show(durationForShow, "swing");
}

function hideMoreInfo() {
    $(this)
        .find(nameAdditionalInformationClassName)
        .hide(durationForShow, "swing");
}

function hideAllMoreInfo() {
    let moreInfo = $(nameAdditionalInformationClassName);
    $.each(moreInfo, function () {
        let isHidden = $(this).is(":hidden");
        if (!isHidden) {
            $(this).hide();
        }
    });
}

function prepareFilms() {
    hideAllMoreInfo();

    let cards = $(".film-card");
    $.each(cards, function () {
        $(this).hover(displayMoreInfo, hideMoreInfo);
    });
}

$(document).ready(function () {
    prepareFilms();
});


function handleCardButtons(button, e){
    e.preventDefault()
    if ($(button).data("auth") === "False")
        $('#modal1').modal('show');
    else {
        let film_card = $(button).closest(".film-card__more-info-container")
        let filmId = film_card.attr("href").split('/').pop();
        if ($(button).data("type") === "like") {
            let likeBtn = film_card.find("#like_title");
            getGrade(filmId, likeBtn);
        }
        else {
            let watchLaterBtn = film_card.find("#watch_later_title");
            let isAdded = $(button).data("added");
            if(isAdded === "True") {
                RemoveFilmFromWatchLater(filmId, watchLaterBtn)
                $(button).data("added", "False");
                watchLaterBtn.attr("title", watchLaterAddText)
            }
            else {
                AddToWatchLater(filmId, watchLaterBtn);
                $(button).data("added", "True");
                watchLaterBtn.attr("title", watchLaterRemoveText)
            }
        }
    }
}


$(".film_card_btns").on('click', function (e) {
    handleCardButtons(this, e)
});

$("#container_film_card").on("click", ".film_card_btns", function (e) {
    handleCardButtons(this, e)
});

