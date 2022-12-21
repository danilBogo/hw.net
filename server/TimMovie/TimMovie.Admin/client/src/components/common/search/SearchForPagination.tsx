import Search from "./Search";
import {useEffect, useRef} from "react";
import SearchForPaginationProps from "./SearchForPaginationProps";

function SearchForPagination({pagination, ...props}:SearchForPaginationProps){
    let searchBar = useRef<HTMLInputElement>(null);
    
    function trySearch(e: KeyboardEvent) : void{
        if (e.key !== "Enter" ||
            document.activeElement !== searchBar.current){
            return;
        }

        search();
    }

    function search(){
        resetStateForSearch();
        pagination.setFetching(true);
    }

    function resetStateForSearch(): void{
        props.stringForSearch.current.namePart = searchBar.current!.value;
        pagination.reset();
    }

    useEffect(() => {
        window.addEventListener("keypress", trySearch);

        return function (){
            window.removeEventListener("keypress", trySearch);
        }
    }, []);
    
    return (
      <Search label={props.label} onClickSearchBtn={search} ref={searchBar} className={props.className}/>  
    );
}

export default SearchForPagination;