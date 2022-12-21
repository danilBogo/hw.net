export default interface ValidationWithError<TValue>{
    readonly valueIsValid: ((value: TValue) => boolean)
    readonly errorMessage?: string;
}