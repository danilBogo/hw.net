const prev = document.querySelectorAll('.prev');
const next = document.querySelectorAll('.next');

const track = $('.track');

track.each(number => {
    let index = 0;
    let item = track[number];

    function getCardsAmount() {
        let windowWidth = window.innerWidth;
        let cardsOnSlide;
        switch (true) {
            case (windowWidth > 1400):
                cardsOnSlide = 8
                break;
            case (windowWidth > 1200):
                cardsOnSlide = 7
                break;
            case (windowWidth > 1000):
                cardsOnSlide = 6
                break;
            case (windowWidth > 800):
                cardsOnSlide = 5
                break;
            case (windowWidth > 600):
                cardsOnSlide = 4
                break;
            case (windowWidth > 449):
                cardsOnSlide = 3
                break;
            case (windowWidth > 300):
                cardsOnSlide = 2
                break;
            default:
                cardsOnSlide = 1
        }
        return cardsOnSlide;
    }

    let cardsOnSlideAmount = getCardsAmount();
    let columnWidth = $('.card-container').outerWidth();

    function getTransformValue() {
        const style = window.getComputedStyle(track[number])
        const matrix = style.transform
        const matrixValues = matrix.match(/matrix.*\((.+)\)/)[1].split(', ')
        return parseFloat(matrixValues[4]);
    }

    function setMoveCoefficients() {
        shiftCards = cardsOnSlideAmount - 1;
        m = parseInt(16 / shiftCards) - 1;
        ost = 16 - m * shiftCards;

        if (ost < cardsOnSlideAmount) {
            shifts = m;
            ost = ost + shiftCards - cardsOnSlideAmount;
        } else if (ost === cardsOnSlideAmount) {
            shifts = m + 1;
            ost -= cardsOnSlideAmount
        } else {
            shifts = m + 1;
            ost = ost - shiftCards - 1
        }
        isNeedToHide = ost === 0;
    }

//amount of full shifts
    let shifts;
//How many cards will be moved
    let shiftCards;
    let m;
    let ost;
    let isNeedToHide;

    setMoveCoefficients();


    next[number].addEventListener('click', function (e) {
        index++;
        prev[number].style.cssText = "display: flex";
        if (cardsOnSlideAmount === 1) {
            item.style.transform = (`translateX(-${index * columnWidth * 1}px)`);
            if (index === 15) next[number].style.cssText = "display: none";
        } else if (index < shifts) {
            item.style.transform = (`translateX(-${index * columnWidth * shiftCards}px)`);
            if (shifts - 1 === index && isNeedToHide) next[number].style.cssText = "display: none";
        } else {
            item.style.transform = (`translateX(-${-getTransformValue() + ost * columnWidth}px)`)
            next[number].style.cssText = "display: none";
        }
    });

    let flag = ost === 0;

    prev[number].addEventListener('click', function (e) {
        index--;
        next[number].style.cssText = "display: flex";
        if (index === 0) {
            prev[number].style.cssText = "display: none";
        }
        if (cardsOnSlideAmount === 1)
            item.style.transform = (`translateX(-${index * columnWidth}px)`);
        else if (!flag)
            item.style.transform = (`translateX(-${index * columnWidth * shiftCards}px)`);
        else {
            item.style.transform = (`translateX(-${getTransformValue() - ost * columnWidth}px)`);
            flag = true;
        }
    });

    window.addEventListener('resize', () => {
        columnWidth = $('.card-container').outerWidth()
        cardsOnSlideAmount = getCardsAmount();
        setMoveCoefficients();
    })
})