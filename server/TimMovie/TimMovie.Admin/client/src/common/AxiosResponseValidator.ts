import {AxiosResponse} from "axios";

export module AxiosResponseValidator {
    export function checkSuccessResponseStatusAndLogIfError(response: AxiosResponse): boolean{
        let statusCode = response.status.toString();
        let isSuccessResponse = !(statusCode.startsWith("5") && statusCode.startsWith("4"));
        if (!isSuccessResponse){
            console.error(`При обращениии по ${response.config.url} произошла ошибка. Статус: ${response.status}. 
                        Текст статуса: ${response.statusText}`);
        }
        
        return isSuccessResponse;
    }
}