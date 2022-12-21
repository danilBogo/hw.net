import React, {useEffect, useRef, useState} from "react";
import dropdownClasses from "./dropdownWithSearch.module.css";
import DropdownWithSearchProps from "./DropdownWithSearchProps";
import $api from "../../../http";
import {classNameConcat} from "../../../common/classNameConcat";


function DropdownWithSearch<T>({showMenu, setShowMenu, ...props}: DropdownWithSearchProps<T>){
    const [foundActors, setFoundActors] = useState<T[]>([]);
    const [numberOfLoadedRecords, setNumberOfLoadedRecords] = useState(0);
    const [fetching, setFetching] = useState(true);
    const allLoaded = useRef(false);
    const pagination = props.pagination;
    const searchBar = useRef<HTMLInputElement>(null);
    const inputTextForSearch = useRef("");
    const dropdown = useRef<HTMLDivElement>(null);

    function toggleDropdownMenu(): void{
        if (props.onClickDropdown){
            props.onClickDropdown();
        }
        setShowMenu(!showMenu);
    }

    function tryLoadMoreActors(e: any): void {
        let target = e.target;
        const offsetHeight = target.scrollHeight;
        const innerHeight = target.clientHeight;
        const scrollY = target.scrollTop;
        
        setFetching(1.5 * innerHeight >= offsetHeight - scrollY && !allLoaded.current);
    }

    function trySearch(e: KeyboardEvent) : void{
        if (e.key !== "Enter" ||
            document.activeElement !== searchBar.current){
            return;
        }

        search();
    }
    

    function search(){
        resetStateForSearch();
        setFetching(true);
    }

    function resetStateForSearch(): void{
        inputTextForSearch.current = searchBar.current!.value;
        setFoundActors([]);
        setNumberOfLoadedRecords(0);
        allLoaded.current = false;
    }
    
    function onClickMouse(e: any){
        if (!dropdown.current!.contains(e.target)){
            setShowMenu(false);
        }
    }

    useEffect(()=>{
        if (!fetching){
            return;
        }

        let url = `${props.url}?namePart=${inputTextForSearch.current}&skip=${numberOfLoadedRecords}&take=${pagination}`;
        $api.get(url)
            .then(response => {
                if(response.status.toString().startsWith("5")){
                    console.error(`При обращениии по ${url} произошла ошибка. Статус: ${response.status}. 
                        Текст статуса: ${response.statusText}`);
                    return;
                }
                setFoundActors([...foundActors, ...response.data]);
                allLoaded.current = response.data.length < pagination;
                setNumberOfLoadedRecords(numberOfLoadedRecords + response.data.length);
            })
            .finally(() => setFetching(false))
    }, [fetching]);
    

    useEffect(() => {
        window.addEventListener("keypress", trySearch);
        window.addEventListener("click", onClickMouse);
        
        return function (){
            window.removeEventListener("keypress", trySearch);
        }
    }, []);
    
    return (
        <div className={dropdownClasses.dropdownWithSearch} ref={dropdown}>
            <div className={dropdownClasses.selectField} onClick={toggleDropdownMenu}>
                <div>{props.title}</div>
            </div>
            {(showMenu &&
                <div className={dropdownClasses.menuContainer}>
                    <input ref={searchBar} 
                           className={classNameConcat("form-control-sm", dropdownClasses.search)}/>
                    <div className={dropdownClasses.menuItemsContainer} onScroll={tryLoadMoreActors}>
                        {foundActors.map(props.parsedResultFunction)}
                    </div>
                </div>)}
        </div>);
}

export default DropdownWithSearch;