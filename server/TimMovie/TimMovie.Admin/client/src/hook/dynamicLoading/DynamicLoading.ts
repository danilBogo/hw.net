import {MutableRefObject} from "react";

export default interface DynamicLoading<TQuery>{
    readonly url: string;
    readonly pagination: number;
    readonly urlQuery?: MutableRefObject<any>;
}