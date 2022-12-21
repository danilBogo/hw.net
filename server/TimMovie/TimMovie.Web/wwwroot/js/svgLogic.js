AddToWatchLaterSvg = document.createElementNS("http://www.w3.org/2000/svg", "use");
AddToWatchLaterSvg.setAttributeNS("http://www.w3.org/1999/xlink", 'xlink:href', "/img/svgIcons/sprite.svg#AddToWatchLater");

RemoveFromWatchLater = document.createElementNS("http://www.w3.org/2000/svg", "use");
RemoveFromWatchLater.setAttributeNS("http://www.w3.org/1999/xlink", 'xlink:href', "/img/svgIcons/sprite.svg#RemoveFromWatchLater");

Like = document.createElementNS("http://www.w3.org/2000/svg", "use");
Like.setAttributeNS("http://www.w3.org/1999/xlink", 'xlink:href', "/img/svgIcons/sprite.svg#Like");

Unlike = document.createElementNS("http://www.w3.org/2000/svg", "use");
Unlike.setAttributeNS("http://www.w3.org/1999/xlink", 'xlink:href', "/img/svgIcons/sprite.svg#Unlike");

function ChangeLikeSvg(isFillRed, likeButton = null) {
    let likeSvg = likeButton === null? $('.svg-grade') : likeButton.find('.svg-grade');
    let use = likeSvg.find("use")
    let useHref = use.attr('xlink:href').split('#');
    let href = useHref.shift();
    let svgName = useHref.pop();
    if (svgName === "Like") {
        likeSvg.css("fill", `${isFillRed ? "#ef233c" : "#c6c6c9"}`);
        use.attr('xlink:href', `${href}#Unlike`);
    } else {
        likeSvg.css("fill", "#c6c6c9");
        use.attr('xlink:href', `${href}#Like`);
    }
}

function ChangeWatchLaterSvg(watchLaterBtn) {
    if (watchLaterBtn === null) watchLaterBtn = $('.watch_later_svg');
    let use = watchLaterBtn.find("use")
    let useHref = use.attr('xlink:href').split('#');
    let href = useHref.shift();
    let svgName = useHref.pop();
    if (svgName === "AddToWatchLater")
        use.attr('xlink:href', `${href}#RemoveFromWatchLater`);
    else
        use.attr('xlink:href', `${href}#AddToWatchLater`);
}