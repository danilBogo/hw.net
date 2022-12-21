import PaginationResult from "../../../hook/dynamicLoading/PaginationResult";
import {MutableRefObject} from "react";
import NamePart from "../../../common/interfaces/NamePart";
import NameDto from "../../../dto/NameDto";

export default interface TableWithGenresProps {
    readonly paginationResult: PaginationResult<NameDto>;
    readonly urlQuery: MutableRefObject<NamePart>;
}