$(document).ready(function (){
    uploadBannerImages();
})

$(window).resize(function () {
    uploadBannerImages()
})

function uploadBannerImages(){
    if ($(window).outerWidth() < 1000) {
        getImagesForBanners(true)
    }
    else{
        getImagesForBanners(false)
    }
}


function getImagesForBanners(isSmall) {
    $.post({
        url: "/MainPage/GetImagesForBanners",
        data: {filmIds: getFilmIds(), isSmall: isSmall},
        success: function (data) {
            if (data != null) {
                console.log(data)
                $(".overlay-image").each(function (index){
                    $(this).css("background-image", `url(${data[index]})`);
                })    
            }
        }
    })
}

function getFilmIds() {
    let idsSet = new Set();
    let filmBtns = $("#bannerCarousel").find(".carousel-item").find('.btn')
    filmBtns.each(function () {
        idsSet.add($(this).attr("href").split("/").pop());
    })
    return Array.from(idsSet);
}