(function (){
   const areaForDrop = document.getElementById("dropArea");
   const imgProfileInput = $("#imgProfileInput");
   const errorMessageContainer = $(".edit-profile-modal__error-img");
   const editProfileImgModal = $('#editProfileImg');
   const saveImgProfileBtn = $("#saveProfileImgBtn");
   const currentImg = $(".edit-profile-modal__current-img");
   const userPhotoInProfile = $(".profile-card__img");
   const messageOnSaveUserPhoto = $("#messageOnSave");
   const textMessageOnSave = $("#textMessageOnSave");
   const oneMbInByte = 1024 * 1024;
   
    function highlight(e) {
        areaForDrop.classList.add("edit-profile-modal__add-img-container_active");
    }
    function unhighlight(e) {
        areaForDrop.classList.remove('edit-profile-modal__add-img-container_active');
    }
    function preventDefaults (e) {
        e.preventDefault();
        e.stopPropagation();
        console.log("preventDefaults")
    }
    function handleDrop(e) {
        console.dir(e);
        let files = e.dataTransfer.files
        loadImage(files)
    }
    function outputError(errorMessage){
        errorMessageContainer.text(errorMessage);
        imgProfileInput.val(null);
    }
    function loadImage(files){
        console.log(files);
        let file = files[0];
        if (!file.type.match('image.(jpeg|jpg|pjpeg|png)')) {
            outputError("Фотография может иметь только следующие форматы: .jpeg, .jpg, .pjpeg, .png");
            return;
        } else if (file.size > oneMbInByte) {
            outputError("Размер фотографии не должен превышать 1 Мб");
            return;
        } else {
            clearErrorMessageForProfileImg();
        }
        const fr = new FileReader();

        fr.onload = (function(file) {
            const img = document.createElement('img');
            img.onload = function () {
                console.dir(img);
                currentImg.attr("src", this.src);
                imgProfileInput.files = files;
                saveImgProfileBtn.attr("disabled", false);
            };
            img.src = file.target.result;
        })
        
        fr.readAsDataURL(file);
    }
    function tryShowImage(e) {
        loadImage(e.target.files);
    }
    function clearErrorMessageForProfileImg(){
        errorMessageContainer.text("");
    }
    function saveUserPhoto(){
        saveImgProfileBtn.attr("disabled", true);
        let formData = new FormData();
        formData.append('file', imgProfileInput.files[0]);

        $.ajax({
            type: "POST",
            url: '/UserProfile/SaveUserPhoto',
            cache: false,
            contentType: false,
            processData: false,
            data: formData,
            dataType : 'json',
            success: function(response){
                if (!response.succeeded){
                    saveImgProfileBtn.attr("disabled", false);
                    outputError(response?.error 
                        ?? "Фотогорафия не соотвутствует требованиям, попробуйте еще раз;");
                    return;
                }
                
                userPhotoInProfile.attr("src", currentImg.attr("src"));
                editProfileImgModal.modal("hide");
                textMessageOnSave.text("Фотография успешно сохранена");
                messageOnSaveUserPhoto.toast("show");
            }
        });
    }
    
    window.addEventListener("dragover",function(e){
        e.preventDefault();
    },false);
    
    window.addEventListener("drop",function(e){
        e.preventDefault();
    },false);
    
    ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
        areaForDrop.addEventListener(eventName, preventDefaults, false);
    });

    ['dragenter', 'dragover'].forEach(eventName => {
        areaForDrop.addEventListener(eventName, highlight, false);
    });
    
    ['dragleave', 'drop'].forEach(eventName => {
        areaForDrop.addEventListener(eventName, unhighlight, false);
    });
    
    areaForDrop.addEventListener('drop', handleDrop, false);

    imgProfileInput.on("change", tryShowImage);
    $("#dropArea").on("click",function (){
        imgProfileInput.click();
    });
    
    editProfileImgModal.on('hidden.bs.modal', clearErrorMessageForProfileImg);
    saveImgProfileBtn.on("click", saveUserPhoto);
})()