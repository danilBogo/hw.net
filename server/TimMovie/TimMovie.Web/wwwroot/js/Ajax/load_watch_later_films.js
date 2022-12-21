(function () {
    let cardContainer = $(".watch_later_films");
    let amountSkip = 0;
    let amountTake = 10;
    let allLoaded = false;
    let isLoad = false;
    let currentRequest;
    let requestIsAlreadySent = false;

    function getWatchLaterFilms() {
        if (!requestIsAlreadySent) {
            $('.loader').toggleClass('hide');
        }

        requestIsAlreadySent = true;
        currentRequest = $.post({
            url: "/WatchLater/WatchLaterFilms/",
            data: {Take: amountTake, Skip: amountSkip},
            success: function (result) {
                $('.loader').toggleClass('hide');
                requestIsAlreadySent = false;

                amountSkip += amountTake;
                allLoaded = result.length < 10;

                cardContainer.append(result);
                isLoad = false;
                if ($(".watch_later_films").find(".movie_card").length === 0)
                    $(".watch_later_films_empty_text").text("Вы ещё не добавили фильмы в смотреть позже")
            }
        })
    }

    function tryLoadMoreFilms() {
        if (allLoaded) {
            return;
        }

        const height = document.body.offsetHeight;
        const screenHeight = window.innerHeight;

        const scrolled = window.scrollY;

        const threshold = height - screenHeight * 2;

        const position = scrolled + screenHeight;

        if (position >= threshold && !isLoad) {
            isLoad = true;
            getWatchLaterFilms();
        }
    }

    $(document).ready(function () {
        tryLoadMoreFilms();
        window.addEventListener("scroll", tryLoadMoreFilms);
    });

})()