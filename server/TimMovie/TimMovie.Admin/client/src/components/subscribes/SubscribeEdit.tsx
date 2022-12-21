import $api from "../../http";
import Result from "../../dto/Result";
import ModalWindow from "../common/modal/ModalWindow";
import useModalWindow from "../../hook/modal/useModalWindow";
import ToastNotification from "../common/modal/ToastNotification";
import {useNavigate, useParams} from "react-router-dom";
import React, {useEffect, useState} from "react";
import {AxiosResponse} from "axios";
import SubscribeAllInfoDto from "../../dto/SubscribeAllInfoDto";
import SubscribeForm from "./SubscribeForm";
import SubscribeDto from "./props/SubscribeDto";
import {ResultActionForUser} from "../../dto/ResultActionForUser";

function SubscribeEdit() {
    const {id: subscribeId} = useParams();
    const navigate = useNavigate();
    const modalErrorMessageAboutSubscribeDownload = useModalWindow("При загрузке подписки произошла ошибка. " +
        "Возможно такой подписки не существует.");
    const modalErrorMessageAboutSubscribeUpdate = useModalWindow();
    const messageAboutUpdateSubscribe = useModalWindow("Подписка успешно обновлена");
    const [formInitialization, setFormInitialization] = useState<SubscribeAllInfoDto>();

    async function updateFilm(data: SubscribeDto): Promise<boolean>{
        let response = await $api.post<ResultActionForUser>(`/subscribes/update/${subscribeId}`, data);

        if (!response.data.success){
            modalErrorMessageAboutSubscribeUpdate.setMessageText(response.data.textMessageForUser
                ?? "Произошла неизвестная ошибка");
            modalErrorMessageAboutSubscribeUpdate.setMessageIsShow(true);
            return false;
        }

        messageAboutUpdateSubscribe.setMessageIsShow(true);
        return true;
    }

    useEffect(() => {
        $api.get(`/subscribes/collection/${subscribeId}`).then((r: AxiosResponse<Result<SubscribeAllInfoDto>>) =>{
            if (!r.data.success){
                console.warn(r.data.textError);
                modalErrorMessageAboutSubscribeDownload.setMessageIsShow(true);
                return;
            }

            setFormInitialization(r.data.result);
        })
    }, []);

    return (
        <>
            <ModalWindow modalControl={modalErrorMessageAboutSubscribeDownload} headerText="Ошибка"
                         headerClass="bg-danger" onHide={() => {navigate("/subscribes/collection")}}/>
            <ModalWindow modalControl={modalErrorMessageAboutSubscribeUpdate} headerText="Ошибка"
                         headerClass="bg-danger"/>
            <ToastNotification modalControl={messageAboutUpdateSubscribe}/>
            <div className="d-flex flex-column mt-4">
                <SubscribeForm isEdit saveSubscribe={updateFilm} initializationForm={formInitialization}/>
            </div>
        </>
    );
}

export default SubscribeEdit;