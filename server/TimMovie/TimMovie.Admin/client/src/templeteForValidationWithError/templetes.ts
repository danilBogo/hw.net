import ValidationWithError from "../hook/validation/ValidationWithError";

export const checkOnEmpty: ValidationWithError<string> = { 
    valueIsValid: value => !!value,
    errorMessage: "Это поле должно быть заполнено"
} 