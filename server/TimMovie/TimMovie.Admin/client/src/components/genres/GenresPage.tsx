import GenreFormAdd from "./GenreFormAdd";
import TableWithGenres from "./TableWithGenres";
import {useRef} from "react";
import NamePart from "../../common/interfaces/NamePart";
import usePagination from "../../hook/dynamicLoading/usePagination";
import NameDto from "../../dto/NameDto";

function GenresPage(){
    let urlQuery = useRef<NamePart>({namePart: ""});
    const genres = usePagination<NameDto, NamePart>({pagination: 30, url: "/genres/collection", urlQuery: urlQuery})
    
    return (
        <div className="mt-4">
            <GenreFormAdd setFetching={genres.setFetching} resetTable={genres.reset}/>
            <TableWithGenres paginationResult={genres} urlQuery={urlQuery}/>
        </div>
    );
}

export default GenresPage;