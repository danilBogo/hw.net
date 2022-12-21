import React, {useRef} from "react";
import FilmForm from "./FilmForm";
import filmPageClasses from "./filmPage.module.css"
import TableWithFilms from "./TableWithFilms";
import NamePart from "../../common/interfaces/NamePart";
import usePagination from "../../hook/dynamicLoading/usePagination";
import FilmForTableDto from "../../dto/FilmForTableDto";
import AddFilmForm from "./AddFilmForm";

function FilmsPage() {
    let urlQuery = useRef<NamePart>({namePart: ""});
    const pagination = usePagination<FilmForTableDto, NamePart>({pagination: 30, url: `/films/pagination`, urlQuery});
    
    return (
        <div className="d-flex flex-column mt-4">
            <div className={filmPageClasses.addFormContainer}> 
                <AddFilmForm resetTable={pagination.reset} setFetching={pagination.setFetching}/>
            </div>
            <TableWithFilms paginationResult={pagination} urlQuery={urlQuery}/>
        </div>
    );
}

export default FilmsPage;