import SubscribeAddForm from "./SubscribeAddForm";
import "./styles/subscribePage.css"
import TableWithSubscribes from "./TableWithSubscribes";
import {useRef} from "react";
import NamePart from "../../common/interfaces/NamePart";
import usePagination from "../../hook/dynamicLoading/usePagination";
import SubscribeInfoDto from "../../dto/SubscribeInfoDto";

function SubscribesPage(){
    let urlQuery = useRef<NamePart>({namePart: ""});
    const pagination = usePagination<SubscribeInfoDto, NamePart>({pagination: 30, url: `/subscribes/pagination`, urlQuery});
    
    return (
      <div>
          <div className={"subscribe_table"}>
              <SubscribeAddForm setFetching={pagination.setFetching} resetTable={pagination.reset}/>
          </div>
          <TableWithSubscribes urlQuery={urlQuery} paginationResult={pagination}/>
      </div>  
    );
}

export default SubscribesPage;