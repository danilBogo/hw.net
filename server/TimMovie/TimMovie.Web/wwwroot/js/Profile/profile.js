(function (){
    const editImgButton = $(".profile-card__edit-img-btn");
    const durationForShowEditImgBtn = 100;
    
    function showEditImgButton(){
        editImgButton.show(durationForShowEditImgBtn, "swing");
    }

    function hideEditImgButton(){
        editImgButton.hide(durationForShowEditImgBtn, "swing");
    }
    
    $(document).ready(function (){
        editImgButton.hide();
        $(".profile-card__img-container").hover(showEditImgButton, hideEditImgButton);
    });
})()