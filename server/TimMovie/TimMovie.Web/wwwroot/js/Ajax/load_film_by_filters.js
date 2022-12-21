(function (){
    let cardContainer = $("#container_film_card");
    let amountSkip = 0;
    let amountTake = 30;
    let allLoaded = false;
    let isLoad = false;
    let currentRequest;
    let requestIsAlreadySent = false;

    function getObjWithFilters(){
        let sortingType = $("#sort-type").val();
        let isDescending = $("#sort-order").is(':checked');

        let genresName = []; 
        let genres = $("#genre-filter").find(".more-filters_list-item_active");
        $.each(genres, function (i, item){
            genresName.push(item.dataset.value); 
        });

        let period = $("#year-filter").find(".more-filters_list-item_active").first()[0];
        let annualPeriod = {
            firstYear: period.dataset.valueFirst,
            lastYear: period.dataset.valueLast
        }

        let countriesName = [];
        let countries = $("#country-filter").find(".more-filters_list-item_active");
        $.each(countries, function (i, item){
            countriesName.push(item.dataset.value);
        });

        let ratingObj = $("#rating-filter").find(".more-filters_list-item_active")[0];
        let rating = ratingObj === undefined ? null : ratingObj.dataset.value;

        return {
            sortingType,
            genresName,
            annualPeriod,
            countriesName,
            rating,
            isDescending
        }
    }

    function getFilmsByFilters(){
        let infoAboutFilters = getObjWithFilters();
        let data = {
            filtersWithPagination: {
                dataDto: infoAboutFilters,
                amountSkip,
                amountTake,
            }
        };

        if (!requestIsAlreadySent){
            $('.loader').toggleClass('hide');   
        }

        requestIsAlreadySent = true;
        currentRequest = $.post({
            url: "/Films/FilmFilters",
            data: data,
            success: function (result){
                console.log("Написал, результат пришел")
                $('.loader').toggleClass('hide');
                requestIsAlreadySent = false;

                amountSkip += amountTake;
                allLoaded = result.length < 30;

                cardContainer.append(result);
                $("img").one("load", function() {
                    prepareFilms();
                    adaptContainer();
                });
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

    function loadWithNewFilter(){
        currentRequest.abort();
        cardContainer.empty();
        amountSkip = 0;
        getFilmsByFilters();
    }

    $(document).ready(function (){
        tryLoadMoreFilms();

        $(".dropdown-filter__list-item").on("click", loadWithNewFilter);
        $(".more-filters__list-item").on("click", loadWithNewFilter);
        $("#sort-order").on("click", loadWithNewFilter);
        window.addEventListener("scroll", tryLoadMoreFilms);
        window.addEventListener("resize", tryLoadMoreFilms);
    });
})()

