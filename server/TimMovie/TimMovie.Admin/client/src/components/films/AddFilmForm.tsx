import FilmForm from "./FilmForm";
import $api from "../../http";
import {AxiosResponseValidator} from "../../common/AxiosResponseValidator";
import ModalWindow from "../common/modal/ModalWindow";
import filmFormClasses from "./filmForm.module.css";
import ToastNotification from "../common/modal/ToastNotification";
import React from "react";
import useModalWindow from "../../hook/modal/useModalWindow";
import AddFilmFormProps from "./AddFilmFormProps";

function AddFilmForm({resetTable, setFetching}: AddFilmFormProps){
    const modalControl = useModalWindow();
    const messageAboutAddFilm = useModalWindow("Фильм успешно добавлен");

    async function tryAddFilm(data: FormData): Promise<boolean>{
        let response = await $api.post("/films/add", data);

        if(!AxiosResponseValidator.checkSuccessResponseStatusAndLogIfError(response)){
            return false;
        }

        if (!response.data.success){
            modalControl.setMessageText(response.data.textError ??
                "Произошла неизвестная ошибка, перезагрузите страницу и попробуйте снова");
            modalControl.setMessageIsShow(true);
            return false;
        }

        messageAboutAddFilm.setMessageIsShow(true);
        resetTable();
        setFetching(true);
        
        return true;
    }
    
    return (
        <>
            <ModalWindow modalControl={modalControl} headerText="Ошибка"
                         headerClass={filmFormClasses.errorMessageHeader}/>
            <ToastNotification modalControl={messageAboutAddFilm}/>
            <FilmForm trySaveFilm={tryAddFilm} isEdit={false}/>
        </>
    )
}

export default AddFilmForm;