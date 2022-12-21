
    const containerForCardFilm = $("#container_film_card");
    let currentClassForContainer = "";

    //Меняет количество столбцов в контейнере, который содержит все карточки фильмов.
    function changeAmountOfCol() {
        containerForCardFilm.removeClass(currentClassForContainer);

        let width = window.innerWidth;

        currentClassForContainer =
            width <= 260 ?
            "row-cols-1" :
            width <= 500 ?
            "row-cols-2" :
            width <= 680 ?
            "row-cols-3" :
            width <= 840 ?
            "row-cols-4" :
            width <= 1000 ?
            "row-cols-5" :
            "row-cols-6";

        containerForCardFilm.addClass(currentClassForContainer);
    }

    function getFontSizeByImageWidth(width) {
        return (width / 100) * 10;
    }

    // Меняет размер названия фильма в карточке в зависимости от размера картинки в карточке.
    function changeFontSizeTitleCardFilms() {
        let images = $(".film-card__img");

        if (images.length === 0) {
            return;
        }
        let image = images[0]
        
        if (image.offsetWidth !== 0){
            resizeText(image.offsetWidth);
            return;
        }

        $(image).one("load", function (e) {
            let width = e.currentTarget.offsetWidth;
            resizeText(width)
        });
    }
    
    function resizeText(width){
        let newFontSizeForTitle = getFontSizeByImageWidth(width);

        let allCards = $(".film-card");

        $.each(allCards, function() {
            $(this).css("font-size", `${newFontSizeForTitle}px`);
        });
    }

    function adaptContainer() {
        changeAmountOfCol();
        changeFontSizeTitleCardFilms();
    }

    $(document).ready(function() {
        adaptContainer();

        $(window).resize(adaptContainer);
    });