let choseButton = null;
let gradeNumber = $(".gradeNumber");
let savedFilmId = null;
let gradeSetText = "Изменить оценку фильма";
let gradeUnsetText = "Поставить оценку фильму";
let likeButton = null;



function getGrade(filmId, likeBtn = null) {
    likeButton = likeBtn;
    savedFilmId = filmId;
    gradeNumber.css("background", "#1e1a2e");
    return $.post({
        url: "/Film/GetGrade",
        data: {filmId: filmId},
        success: function (data) {
            if (data != null) {
                let grade = Number(data);
                if (!isNaN(grade)) {
                    choseButton = $(`.gradeNumber:contains(${grade})`).first();
                    choseButton.css("background", "#302a45");
                    return grade;
                }
            }
        }
    });

}

function setGrade(filmId, e, inCardGrade = false, watchedFilmsLike = null) {
    return $.post({
        url: "/Film/SetGrade",
        data: {filmId: savedFilmId, grade: e.target.innerText},
        success: function () {
            gradeNumber.css("background", "#1e1a2e");
            $(e.target).css("background", "#302a45");
            $('#modalFilmGrade').modal('hide');
            if (choseButton != null && choseButton[0].innerText === $(e.target)[0].innerText){
                if (watchedFilmsLike !== null)
                {
                    watchedFilmsLike.innerText="-";
                }
                else {
                    choseButton = null;
                    inCardGrade ? updateFilmCardAfterUnset(likeButton) : updateGradeAfterUnset();
                }
            }
            else if (choseButton != null && choseButton[0].innerText !== $(e.target)[0].innerText){
                choseButton = $(e.target);
            }
            else{
                choseButton = $(e.target);
                if(watchedFilmsLike === null)
                    inCardGrade? updateFilmCardAfterSet(likeButton) : updateGradeAfterSet();
            }
        }
    });
}

function updateGradeAfterSet(){
    $("#rate_movie_label").text(gradeSetText) 
    ChangeLikeSvg(true)
}

function updateGradeAfterUnset(){
    $("#rate_movie_label").text(gradeUnsetText) 
    ChangeLikeSvg(true)
}

function updateFilmCardAfterSet(){
    likeButton.attr("title", gradeSetText)
    ChangeLikeSvg(false, likeButton);
}

function updateFilmCardAfterUnset(){
    likeButton.attr("title", gradeUnsetText)
    ChangeLikeSvg(false, likeButton);
}


gradeNumber.click(function (e) {
    likeButton === null ? setGrade(savedFilmId, e) : setGrade(savedFilmId, e, true)
})

gradeNumber.mouseover(function (e) {
    $(e.target).css({
        "background": "#B22222",
        "border-color": "#B22222",
        "cursor": "pointer"
    });
});

gradeNumber.mouseleave(function (e) {
    let background = "#1e1a2e";
    if (choseButton != null && choseButton[0].innerText === $(e.target)[0].innerText)
        background = "#302a45";
    $(e.target).css({
        "background": background,
        "border-color": "#1e1a2e",
        "cursor": "default"
    });
});