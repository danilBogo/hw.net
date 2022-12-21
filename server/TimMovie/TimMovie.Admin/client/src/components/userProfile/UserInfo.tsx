import React, {useEffect, useRef, useState} from 'react';
import EditButtons from "./EditButtons";
import './userProfile.css';
import {IFullUserInfoDto} from "../../dto/IFullUserInfoDto";
import $api from "../../http";
import UserInfoContainer from "./containers/UserInfoContainer";
import Container from "./containers/Container";
import LineWithSvg from "./LineWithSvg";
import UserInfoProps from "./props/UserInfoProps";
import {UserInfoDto} from "../../dto/UserInfoDto";
import ModalWindow from "../common/modal/ModalWindow";
import useModalWindow from "../../hook/modal/useModalWindow";
import ToastNotification from "../common/modal/ToastNotification";

const UserInfo = (props: UserInfoProps) => {
    const [info, setInfo] = useState<IFullUserInfoDto>();
    const selectedRoles = useRef(new Set<string>());
    const lastSelectedRoles = useRef<Set<string>>();
    const selectedSubscribes = useRef<Set<string>>(new Set<string>());
    const lastSelectedSubscribes= useRef<Set<string>>();
    const [formIsEdit , setFormIsEdit] = useState<boolean>(false);
    const errorMessage = useModalWindow();
    const messageAboutUpdate = useModalWindow("Данные успешно обновлены");

    useEffect(() => {
        getAllUserInfo();
    }, [])

    function getAllUserInfo() {
        let url = `/users/user?id=${props.userId}`;
        $api.get<IFullUserInfoDto>(url).then(response => {
            setInfo(response.data);
            selectedRoles.current = new Set(response.data.roles);
            lastSelectedRoles.current = new Set(response.data.roles);
            selectedSubscribes.current = new Set(response.data.subscribes);
            lastSelectedSubscribes.current = new Set(response.data.subscribes);
        });
    }
    
    function generateDataForUpdate(): UserInfoDto{
        return {
            userId: props.userId,
            roleNames: Array.from(selectedRoles.current),
            subscribeNames: Array.from(selectedSubscribes.current)
        }
    }
    
    async function updateUserInfo(){
        let data = generateDataForUpdate();
        $api.post("/users/updateRolesAndSub", data)
            .then(() => {
                messageAboutUpdate.setMessageIsShow(true);
                setFormIsEdit(false);
                lastSelectedRoles.current = new Set(selectedRoles.current);
                lastSelectedSubscribes.current = new Set(selectedSubscribes.current);
            })
            .catch(() => {
                errorMessage.setMessageText("Неизвестная ошибка. Попробуйте перезагрузить и повторить попытку.");
                errorMessage.setMessageIsShow(true);
            });
    }
    
    function cancel(){
        selectedRoles.current = new Set(lastSelectedRoles.current);
        selectedSubscribes.current = new Set(lastSelectedSubscribes.current);
        setFormIsEdit(false);
    }

    return (
        <>
            <ToastNotification modalControl={messageAboutUpdate}/>
            <ModalWindow modalControl={errorMessage} headerText={"Ошибка"} headerClass='bg-danger'/>
            <div className="mt-4">
                <EditButtons change={{clickState: formIsEdit, setClickState: setFormIsEdit}}
                             onSave={updateUserInfo}
                             onCancel={cancel}/>
                <div className="row mt-5">
                    <div className="user_info_container col-12 col-md-3">
                        <UserInfoContainer info={info}/>
                    </div>

                    <div className="containers roles_container col-12 col-md-3 offset-md-1">
                        <LineWithSvg icon={"Roles"} line={"Роли пользователя"} isHeader={true} iconWidth={28}
                                     iconHeight={21}/>
                        <Container isEdit={formIsEdit} iconName="User" namesIncludedInUser={selectedRoles.current}
                                   urlForLoadAllInfo={"/roles/collection"}/>
                    </div>

                    <div className="containers subscribes_container col-12 col-md-4 offset-md-1">
                        <LineWithSvg icon={"Subscribes"} line={"Доступные подписки"} isHeader={true} iconWidth={28} iconHeight={21}/>
                        <Container isEdit={formIsEdit} iconName="Subscribe"
                                   namesIncludedInUser={selectedSubscribes.current}
                                   urlForLoadAllInfo={"/subscribes/collection"}/>
                    </div>
                </div>
            </div>
        </>
    );
};

export default UserInfo;