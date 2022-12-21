import React, {useEffect, useRef, useState} from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import ColumnTable from "./ColumnTable";
import ColumnTableWithList from "./ColumnTableWithList";
import Search from '../common/search/Search';
import {IUserDto} from "../../dto/IUserDto";
import $api from "../../http";

function UsersTablePage(){
    const [users, setUsers] = useState<Array<IUserDto>>([]);
    const [numberOfLoadedRecords, setNumberOfLoadedRecords] = useState(0);
    const [fetching, setFetching] = useState(true);
    const allLoaded = useRef(false);
    const pagination = 30;
    
    const searchBar = useRef<HTMLInputElement>(null);
    const inputTextForSearch = useRef("");

    function tryLoadMoreUsers(): void {
        const offsetHeight = document.body.offsetHeight;
        const innerHeight = window.innerHeight;
        const scrollY = window.scrollY;
    
        setFetching(1.5 * innerHeight >= offsetHeight - scrollY && !allLoaded.current);
    }
    
    function trySearch(e: KeyboardEvent) : void{
        if (e.key !== "Enter" ||
            document.activeElement !== searchBar.current){
            return;
        }
        
        searchByLogin();
    }
    
    function searchByLogin(){
        resetStateForSearch();
        setFetching(true);
    }
    
    function resetStateForSearch(): void{
        inputTextForSearch.current = searchBar.current!.value;
        setUsers([]);
        setNumberOfLoadedRecords(0);
        allLoaded.current = false;
    }
    
    useEffect(()=>{
        if (!fetching){
            return;
        }
        
        let url = `/users/collection?incomingText=${inputTextForSearch.current}&skip=${numberOfLoadedRecords}&take=${pagination}`;
        $api.get(url)
            .then(response => {
                if(response.status.toString().startsWith("5")){
                    console.error(`При обращениии по ${url} произошла ошибка. Статус: ${response.status}. 
                        Текст статуса: ${response.statusText}`);
                    return;
                }
                console.log(response.data);
                setUsers([...users, ...response.data]);
                allLoaded.current = response.data.length < pagination;
                setNumberOfLoadedRecords(numberOfLoadedRecords + response.data.length);
            })
            .finally(() => setFetching(false))
    }, [fetching]);
    
    useEffect(() => {
        window.addEventListener("scroll", tryLoadMoreUsers);
        window.addEventListener("keypress", trySearch);

        return function (){
            window.removeEventListener("scroll", tryLoadMoreUsers);
            window.removeEventListener("keypress", trySearch);
        }
    }, []);
    
    return (
        <div className="d-flex flex-column justify-content-center mt-4">
            <Search ref={searchBar} label='Поиск по логину' onClickSearchBtn={searchByLogin} className="form-control"/>
            <div className="d-flex justify-content-between mb-4 mt-3">
                <ColumnTable nameColumn={"Логин"} users={users} userPropName={"login"} isLinked={true}/>
                <ColumnTable nameColumn={"Почта"} users={users} userPropName={"email"} isLinked={false}/>
                <ColumnTableWithList nameColumn={"Роли"} users={users} userPropName={"roles"} 
                                     messageInEmptyList={"Нет ролей"}/>
                <ColumnTableWithList nameColumn={"Подписки"} users={users} userPropName={"subscribes"} 
                                     messageInEmptyList={"Нет подписок"}/>
            </div>
        </div>
    );
}

export default UsersTablePage;