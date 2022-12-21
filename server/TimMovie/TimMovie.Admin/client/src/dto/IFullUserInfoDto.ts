import {IUserDto} from "./IUserDto";

export interface IFullUserInfoDto extends IUserDto{
    displayName: string;
    registrationDate: Date;
    birthDate: string;
    countryName: string | null;
}