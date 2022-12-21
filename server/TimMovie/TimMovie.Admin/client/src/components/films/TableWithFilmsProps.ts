import PaginationResult from "../../hook/dynamicLoading/PaginationResult";
import FilmForTableDto from "../../dto/FilmForTableDto";
import NamePart from "../../common/interfaces/NamePart";
import {MutableRefObject} from "react";

export default interface TableWithFilmsProps{
    readonly paginationResult: PaginationResult<FilmForTableDto>;
    readonly urlQuery: MutableRefObject<NamePart>;
} 