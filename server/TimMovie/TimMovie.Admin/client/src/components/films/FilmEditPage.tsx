import FilmForm from "./FilmForm";
import React, {useEffect, useState} from "react";
import FullInfoAboutFilmDto from "../../dto/FullInfoAboutFilmDto";
import {useNavigate, useParams} from "react-router-dom";
import $api from "../../http";
import {AxiosResponseValidator} from "../../common/AxiosResponseValidator";
import {AxiosResponse} from "axios";
import Result from "../../dto/Result";
import useModalWindow from "../../hook/modal/useModalWindow";
import ModalWindow from "../common/modal/ModalWindow";
import ToastNotification from "../common/modal/ToastNotification";

function FilmEditPage(){
    const {id: filmId} = useParams();
    const navigate = useNavigate();
    const modalErrorMessageAboutFilmDownload = useModalWindow("При загрузке фильма произошла ошибка. " +
        "Возможно такого фильма не существует.");
    const modalErrorMessageAboutFilmUpdate = useModalWindow();
    const messageAboutUpdateFilm = useModalWindow("Фильм успешно обновлен");
    const [formInitialization, setFormInitialization] = useState<FullInfoAboutFilmDto>();
    
    async function updateFilm(data: FormData): Promise<boolean>{
        let response = await $api.post<Result<string>>(`/films/${filmId}`, data);

        if(!AxiosResponseValidator.checkSuccessResponseStatusAndLogIfError(response)){
            return false;
        }

        if (!response.data.success){
            console.warn(response.data.textError);
            modalErrorMessageAboutFilmUpdate.setMessageText(response.data.textError ?? "Произошла неизвестная ошибка");
            modalErrorMessageAboutFilmUpdate.setMessageIsShow(true);
            return false;
        }

        messageAboutUpdateFilm.setMessageIsShow(true);
        return true;
    }
    
    useEffect(() => {
        $api.get(`/films/${filmId}`).then((r: AxiosResponse<Result<FullInfoAboutFilmDto>>) =>{
            if(!AxiosResponseValidator.checkSuccessResponseStatusAndLogIfError(r)){
                return;
            }
            
            if (!r.data.success){
                console.warn(r.data.textError);
                modalErrorMessageAboutFilmDownload.setMessageIsShow(true);
                return;
            }
            
            setFormInitialization(r.data.result);
        })
    }, []);
    
     return (
         <>
             <ModalWindow modalControl={modalErrorMessageAboutFilmDownload} headerText="Ошибка"
                          headerClass="bg-danger" onHide={() => {navigate("/films/collection")}}/>
             <ModalWindow modalControl={modalErrorMessageAboutFilmUpdate} headerText="Ошибка"
                          headerClass="bg-danger"/>
             <ToastNotification modalControl={messageAboutUpdateFilm}/>
             <div className="d-flex flex-column mt-4">
                 <FilmForm isEdit trySaveFilm={updateFilm} formInitialization={formInitialization}/>
             </div>
         </>
    );
}

export default FilmEditPage;