$(document).ready(function () {
    showOptimalActorCardsNumber();
    if ($(".movie_header").height() > 80){
        $(".movie_header").find("h1").css("font-size", "2rem")
    }
});

function showOptimalActorCardsNumber() {
    let cardsToShow = getOptimalActorCardsNumber();
    for (let i = cardsToShow; i < 4; i++) {
        $(".actor").eq(i).hide();
    }
}

function getOptimalActorCardsNumber() {
    let windowWidth = $(window).width();
    if (windowWidth < 401) return 3;
    else return 4;
}

let isRemoved = false;

$(window).resize(function () {
    if ($(window).width() < 401 && !isRemoved) {
        $(".actor").last().hide();
        isRemoved = true;
    } else if ($(window).width() > 400 && isRemoved) {
        $(".actor").last().show();
        isRemoved = false;
    }
});

$(".watch_later_films").on('click', '.watch_later', function (e) {
    e.preventDefault();
    let filmInfo = $(this).closest(".info_section");
    let filmId = filmInfo.children(".stretched-link").attr("href").split("/").pop();
    RemoveFilmFromWatchLater(filmId, null, true);
    filmInfo.closest("#movie_card").remove()
    if ($(".watch_later_films").find(".movie_card").length === 0)
        $(".watch_later_films_empty_text").text("Вы ещё не добавили фильмы в смотреть позже")
})