import {UserDto} from "./UserDto";

export interface MessageDto {
    content: string,
    user: UserDto,
    time: Date
}

