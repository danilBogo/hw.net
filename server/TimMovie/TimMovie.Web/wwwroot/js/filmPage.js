$('description').ready(function () {
    cropDescriptrion();
})
$(document).ready(function () {
    hideAllActorsAndProducers();
});

let filmIdFromUrl = document.URL.split('/').pop();


$(window).resize(function () {
    moveDescription();
});

function moveDescription() {
    let filmDescription = $("#film_description");
    let windowWidth = $(window).width();
    if (windowWidth > 1199) filmDescription.appendTo("#film_info_container");
    else filmDescription.appendTo("#film-row");
}

function cropDescriptrion() {
    let description = $("#description");
    paragraphs = description.text().trim().split("\n");
    let descLess;
    let descMore;
    if (paragraphs.length > 1) {
        descLess = $("<p>" + paragraphs[0] + "</p>").prop("id", "descLess");
        descMore = $("<div><p>" + paragraphs[1] + "</p></div>").prop("id", "descMore");
        if (paragraphs.length > 2) {
            for (var i = 2; i < paragraphs.length; i++) {
                let paragraph = $("<p>" + paragraphs[i] + "</p>")
                paragraph.appendTo(descMore)
            }
        }
        description.empty();
        descLess.appendTo(description);
        descMore.appendTo(description);
        $("#descMore").hide();
    } else $('#more').hide();
}

function showMore() {
    $("#descMore").show();
}

function showLess() {
    $("#descMore").hide();
}

$("#more").on("click", function () {
    if ($(this).text() === "Читать дальше...") {
        $(this).text("Свернуть");
        showMore();
    } else {
        $(this).text("Читать дальше...");
        showLess();
    }
});

$("#backward_link").on("click", function () {
    history.go(-1);
})

//actors and producers script

const showMoreActorsAndProducersBtn = $("#actorsProducersMoreBtn");
const hideActorsAndProducersBtn = $("#actorsProducersHideBtn");
const durationForShowActors = 400;


function hideAllActorsAndProducers() {
    $(".actors-producers__item__container_show").each(function () {
        $(this).hide();
    })
    hideActorsAndProducersBtn.hide();
}

function showMoreActorsAndProducers() {
    $(".actors-producers__item__container_show").each(function () {
        $(this).show(durationForShowActors);
    })
    showMoreActorsAndProducersBtn.hide();
    hideActorsAndProducersBtn.show(durationForShowActors);
}

function hideActorsAndProducers() {
    $(".actors-producers__item__container_show").each(function () {
        $(this).hide();
    })
    showMoreActorsAndProducersBtn.show(durationForShowActors);
    hideActorsAndProducersBtn.hide();
}

showMoreActorsAndProducersBtn.on("click", showMoreActorsAndProducers);
hideActorsAndProducersBtn.on("click", hideActorsAndProducers);
hideActorsAndProducersBtn.on("click", hideActorsAndProducers);



$("#watch_later").on("click",function (e) {
    e.preventDefault()
    if ($(this).data("auth") === "True") {
        let isAdded = $(this).data("added");
        if(isAdded === "True") {
            RemoveFilmFromWatchLater(filmIdFromUrl, $(this).find(".watch_later_svg"))
            $(this).data("added", "False");
            $("#watch_later_label").text(watchLaterAddText)
        }
        else {
            AddToWatchLater(filmIdFromUrl, $(this).find(".watch_later_svg"));
            $(this).data("added", "True");
            $("#watch_later_label").text(watchLaterRemoveText)
        }
    } else $('#modal1').modal('show');
})

$("#rate_movie").click(function () {
    if ($(this).data("auth") === "True")
        getGrade(filmIdFromUrl);
    else $('#modal1').modal('show');
})