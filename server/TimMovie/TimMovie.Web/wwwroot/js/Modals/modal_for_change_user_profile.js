(function (){
    const displayNameInput = $("#displayNameInput");
    const displayNameError = $("#displayNameError");
    const birthDateInput = $("#birthDateInput");
    const birthDateError = $("#birthDateError");
    const countryNameInput = $("#countryNameInput");
    const saveUserInfoBtn = $("#saveUserInfoBtn");
    const commonError = $(".edit-user-info__common-error");
    const messageOnSaveUserInfo = $("#messageOnSave");
    const textMessageOnSave = $("#textMessageOnSave");
    const modalEditUserProfile = $("#editUserProfileInfo");
    const nicknameInProfile = $(".nickname_in_profile");
    const minBirthDate = new Date(1920, 1, 1);
    
    function validateDisplayName(){
        if (!displayNameIsValid()){
            displayNameError.text("Поле должно быть заполнено");
        }else {
            displayNameError.text("");
        }
    }

    function validateBirthDate(){
        if (!birthDateIsValid()){
            birthDateError.text("Дата рождения не может быть больше н.в.");
        }else {
            birthDateError.text("");
        }
    }
    
    function displayNameIsValid(){
        return displayNameInput.val() !== "";
    }
    
    function birthDateIsValid(){
        let dateStr = birthDateInput.val();
        if (dateStr === ""){
            return true;
        }
        
        let dateInInput =  new Date(dateStr);
        return minBirthDate <= dateInInput && dateInInput <= new Date();
    }
    
    function trySaveUserInfo(){
        if (!allFieldsForUserInfoIsValid()){
            return;
        }
        
        saveUserInfo();
    }
    
    function getUserInfoFromFields(){
        return {
            displayName: displayNameInput.val(),
            birthDate: new Date(birthDateInput.val()).toJSON(),
            countryName: countryNameInput.val()
        }
    }
    
    function saveUserInfo(){
        let data = getUserInfoFromFields();
        saveUserInfoBtn.attr("disabled", true);
        $.post({
            url: "/UserProfile/SaveUserInfo",
            data: data,
            success: function (response){
                if (!response.succeeded){
                    commonError.text(response.error);
                    return;
                }

                nicknameInProfile.text(displayNameInput.val());
                modalEditUserProfile.modal("hide");
                textMessageOnSave.text("Данные успешно сохранены");
                messageOnSaveUserInfo.toast("show");
                saveUserInfoBtn.attr("disabled", false);
            }
        });
    }
    
    function allFieldsForUserInfoIsValid(){
        return displayNameIsValid() && birthDateIsValid();
    }
    
    displayNameInput.on("change", validateDisplayName);
    birthDateInput.on("change", validateBirthDate);
    
    [displayNameInput, birthDateError].forEach(field =>{
        field.on("change", function (){
            if (allFieldsForUserInfoIsValid()){
                saveUserInfoBtn.attr("disabled", false);
            }else {
                saveUserInfoBtn.attr("disabled", true);
            }
        })
    });
    
    saveUserInfoBtn.on("click", trySaveUserInfo);
})()