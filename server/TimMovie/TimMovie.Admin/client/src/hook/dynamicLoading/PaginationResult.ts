import {MutableRefObject} from "react";

export default interface PaginationResult<T> {
    readonly setFetching: (value: boolean) => void;
    readonly reset: () => void;
    readonly records: T[];
    readonly allLoaded: MutableRefObject<boolean>;
    readonly fetching: boolean;
}