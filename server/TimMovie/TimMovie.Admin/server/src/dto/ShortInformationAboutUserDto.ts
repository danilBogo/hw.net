export interface ShortInformationAboutUserDto {
    id: string;
    login: string | null;
    email: string | null;
    roles: string[];
    subscribes: string[];
}