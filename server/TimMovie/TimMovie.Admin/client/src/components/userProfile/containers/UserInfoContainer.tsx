import React, {FunctionComponent} from 'react';
import LineWithSvg from "../LineWithSvg";
import {IFullUserInfoDto} from "../../../dto/IFullUserInfoDto";
import moment from "moment";

interface IUserInfoContainerProps {
    info: IFullUserInfoDto | undefined
}

const UserInfoContainer: React.FC<IUserInfoContainerProps> = ({info}:IUserInfoContainerProps) => {
    return (
        <div className="user_info">
            <LineWithSvg title="Отображаемый ник" icon={"Profile"} line={info?.displayName} isBold={true}/>
            <LineWithSvg title="Логин" icon={"Login"} line={info?.login}/>
            <LineWithSvg title="Почта" icon={"Mail"} line={info?.email}/>
            <LineWithSvg title="Дата рождения" icon={"BirthDate"} line={info?.birthDate ?? "Не указано"}/>
            <LineWithSvg title="Страна проживания" icon={"Country"} line={info?.countryName ?? "Не указано"}/>
            <LineWithSvg title="Дата регистрации" icon={"RegisterDate"} line={moment(info?.registrationDate).format("DD-MM-YYYY HH:mm")}/>
        </div>
    );
};

export default UserInfoContainer;