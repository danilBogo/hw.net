(function (){
    let filmContainer = $("#person-film-container");
    const path = window.location.pathname.split("/");
    const personType = path[1];
    const userId = path[2];
    let amountSkip = 0;
    let amountTake = 20;
    let allLoaded = false;
    let isLoad = false;
    let requestIsAlreadySent = false;

    function getFilmsByFilters(){
        if (!requestIsAlreadySent){
            $('.loader').toggleClass('hide');
        }

        requestIsAlreadySent = true;
        $.get({
            url: `/${personType}/films?id=${userId}&skip=${amountSkip}&take=${amountTake}`,
            success: function (result){
                $('.loader').toggleClass('hide');
                requestIsAlreadySent = false;

                amountSkip += amountTake;
                allLoaded = result.length < 30;

                filmContainer.append(result);
                isLoad = false;
            }
        })
    }

    function tryLoadMoreFilms() {
        if (allLoaded){
            return;
        }

        const height = document.body.offsetHeight;
        const screenHeight = window.innerHeight;

        const scrolled = window.scrollY;

        const threshold = height - screenHeight / 4;

        const position = scrolled + screenHeight;

        if (position >= threshold && !isLoad) {
            isLoad = true;
            getFilmsByFilters();
        }
    }

    $(document).ready(function (){
        tryLoadMoreFilms();

        window.addEventListener("scroll", tryLoadMoreFilms);
    });
})()

