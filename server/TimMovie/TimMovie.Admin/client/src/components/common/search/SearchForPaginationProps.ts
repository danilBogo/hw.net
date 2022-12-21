import PaginationResult from "../../../hook/dynamicLoading/PaginationResult";
import {MutableRefObject} from "react";
import NamePart from "../../../common/interfaces/NamePart";

export default interface SearchForPaginationProps {
    readonly pagination: PaginationResult<any>;
    readonly label: string;
    readonly className: string;
    readonly stringForSearch: MutableRefObject<NamePart>
}