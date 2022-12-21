import {ShortInformationAboutUserDto} from "./ShortInformationAboutUserDto";

export interface AllInformationAboutUserDto extends ShortInformationAboutUserDto{
    displayName: string;
    registrationDate: Date;
    birthDate: string;
    countryName: string | null;
}