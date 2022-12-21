const windowSizeToCutText = 990;
const windowSizeToMedium = 1400;
const windowSizeToSmallFont = 660;
const smallFilmTitleLength = 30;

window.onload = function () {
    applySize();
}

window.addEventListener('resize', function () {
    applySize();
});

function applySize() {
    let width = document.documentElement.clientWidth;
    if (width < windowSizeToCutText) {
        CutText();
    }
    resizeToDefault();
    if (width < windowSizeToMedium) {
        if (width > windowSizeToSmallFont) {

            resizeToMedium();
        } else {
            resizeToSmall();
        }
    }
}

function resizeToMedium() {
    document.querySelectorAll('.film-title').forEach(x => x.style = 'width:35%');
    document.querySelectorAll('.film-duration').forEach(x => x.style = 'width:20%');
}

function CutText() {
    document.querySelectorAll('.film-title-txt').forEach(d => {
        if (d.innerText.length > smallFilmTitleLength) {
            d.innerText = d.innerText.substr(0, smallFilmTitleLength);
            d.innerText += '...';
        }
    })
}

function resizeToSmall() {
    document.querySelector('.films-container').querySelectorAll('span').forEach(x => x.style = 'font-size:12px !important');
    document.querySelector('.navigation').querySelectorAll('span').forEach(x => x.style = 'font-size:10px !important;');
    document.querySelector('.navigation-film-name').style = 'width:60%';
    document.querySelectorAll('.film-duration').forEach(x => x.style = 'width:150px');
}

function resizeToDefault() {
    document.querySelectorAll('.film-title').forEach(x => x.style = 'width: 40%;');
    document.querySelector('.films-container').querySelectorAll('span').forEach(x => x.style = 'font-size:18px !important');
    document.querySelector('.navigation').querySelectorAll('span').forEach(x => x.style = 'font-size:15px !important');
    document.querySelector('.navigation-film-name').style = ' width: 48%;';
    document.querySelectorAll('.film-duration').forEach(x => x.style = 'width: 15%;');
}

let body = $("body");
let lastPressedButton = null;
let lastPressedButtonValue = null

body.on('click', '.setGradeButton', (e) => {
    lastPressedButton = $(e.target)[0];
    lastPressedButtonValue = lastPressedButton.innerText;
    let parent = $(e.target)[0].parentNode;
    let filmId = $(parent).find(".filmIdHidden")[0].innerText;
    getGrade(filmId);
    $('.gradeNumber').off('click');
});

body.on('click', '.gradeNumber', (e) => {
    setGrade(savedFilmId, e, false, lastPressedButton).then(_ => {
        if (lastPressedButton !== null){
            lastPressedButton = $(lastPressedButton);
            let lastSettedGrade = choseButton[0].innerText;
            if (lastSettedGrade === lastPressedButtonValue){
                lastSettedGrade="-";
                lastPressedButton.removeClass("goodGrade");
                lastPressedButton.removeClass("badGrade");
                lastPressedButton.addClass("mediumGrade");
            }
            if (lastSettedGrade >= 1 && lastSettedGrade < 5)
            {
                if(lastPressedButton.hasClass("mediumGrade"))
                    lastPressedButton.removeClass("mediumGrade");
                
                if(lastPressedButton.hasClass("goodGrade"))
                    lastPressedButton.removeClass("goodGrade");
                lastPressedButton.addClass("badGrade");
            }
            if (lastSettedGrade >= 5 && lastSettedGrade < 8){
                if(lastPressedButton.hasClass("badGrade"))
                    lastPressedButton.removeClass("badGrade");
                
                if(lastPressedButton.hasClass("goodGrade"))
                    lastPressedButton.removeClass("goodGrade");
                lastPressedButton.addClass("mediumGrade");
            }
            if (lastSettedGrade >= 8 && lastSettedGrade <= 10){
                if(lastPressedButton.hasClass("badGrade"))
                    lastPressedButton.removeClass("badGrade");
                
                if(lastPressedButton.hasClass("mediumGrade"))
                    lastPressedButton.removeClass("mediumGrade");
                
                lastPressedButton.addClass("goodGrade");
            }
            lastPressedButton[0].innerText = lastSettedGrade;
        }
    });
})
