const interval = setInterval(() => {
    if (!document.URL.includes("Film/")) {
        alert("interval cleared");
        clearInterval(interval);
    }
    else{
        updateStatus();
    }
}, 60000);

function updateStatus() {
    $.get({
        url: `/UserProfile/UpdateUserStatusWatchingFilm?filmId=${document.URL.split("/").pop()}`
    })
}