import PaginationResult from "../../../hook/dynamicLoading/PaginationResult";

export default interface DynamicScrollingProps{
    readonly paginationResult: PaginationResult<any>;
    readonly amountScreenBeforeLoading: number;
    readonly children: JSX.Element;
    readonly className?: string;
}
