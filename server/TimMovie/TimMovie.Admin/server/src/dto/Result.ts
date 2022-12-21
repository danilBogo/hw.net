export interface Result<T> {
    readonly success: boolean;
    readonly textError?: string;
    readonly result?: T;
}