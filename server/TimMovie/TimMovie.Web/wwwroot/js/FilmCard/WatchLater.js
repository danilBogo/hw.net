watchLaterAddText = 'Смотреть позже';
watchLaterRemoveText = 'Убрать из смотреть позже';

function AddToWatchLater(filmId, watchLaterBtn = null) {
    $.post({
        url: "/Film/AddFilmToWatchLater",
        data: {filmId: filmId},
        datatype: Boolean,
        success: function (data) {
            if (data != null) {
                ChangeWatchLaterSvg(watchLaterBtn);
            } 
        }
    })
}

function RemoveFilmFromWatchLater(filmId, watchLaterBtn = null, isBigFilmCard = false) {
    $.post({
        url: "/Film/RemoveFilmFromWatchLater",
        data: {filmId: filmId},
        datatype: Boolean,
        success: function (data) {
            if (data != null && !isBigFilmCard) {
                ChangeWatchLaterSvg(watchLaterBtn);
            } 
        }
    })
}