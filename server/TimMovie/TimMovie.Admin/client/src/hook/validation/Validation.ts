import ValidationWithError from "./ValidationWithError";

export default interface Validation<TValue>{
    readonly validations?: ValidationWithError<TValue>[]
}