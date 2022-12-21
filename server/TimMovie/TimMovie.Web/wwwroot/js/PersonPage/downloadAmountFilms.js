(function (){
    const path = window.location.pathname.split("/");
    const personType = path[1];
    const userId = path[2];
    const amountFilmsContainer = $(".person_amount-films");
    
    function downloadAmountFilms(){
        $.get({
            url: `/film/${personType}/${userId}`,
            success: function (amount){
                let filmWord = inclineWord(amount, ["фильм", "фильма", "фильмов"]);
                amountFilmsContainer.text(`${amount} ${filmWord}`);
            }
        });
    }
    
    downloadAmountFilms();
})()