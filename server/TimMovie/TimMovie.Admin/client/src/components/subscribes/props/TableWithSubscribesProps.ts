import PaginationResult from "../../../hook/dynamicLoading/PaginationResult";
import {MutableRefObject} from "react";
import NamePart from "../../../common/interfaces/NamePart";
import SubscribeInfoDto from "../../../dto/SubscribeInfoDto";

export default interface TableWithSubscribesProps {
    readonly paginationResult: PaginationResult<SubscribeInfoDto>;
    readonly urlQuery: MutableRefObject<NamePart>;
}