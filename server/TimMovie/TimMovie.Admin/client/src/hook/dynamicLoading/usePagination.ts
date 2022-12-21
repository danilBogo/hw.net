import {useEffect, useRef, useState} from "react";
import $api from "../../http";
import PaginationResult from "./PaginationResult";
import DynamicLoading from "./DynamicLoading";

export default function usePagination<TValue, TQuery>(options: DynamicLoading<TQuery>): PaginationResult<TValue>{
    const [records, setRecords] = useState<Array<TValue>>([]);
    const [numberOfLoadedRecords, setNumberOfLoadedRecords] = useState(0);
    const [fetching, setFetching] = useState(true);
    const allLoaded = useRef(false);
    const pagination = options.pagination;

    function reset(): void{
        setRecords([]);
        setNumberOfLoadedRecords(0);
        allLoaded.current = false;
    }
    
    function getParamsForUrl(): string{
        let queryString = "";
        for (let paramName in options.urlQuery?.current){
           queryString += `&${paramName}=${options.urlQuery?.current[paramName]}`;
        }
        
        return queryString;
    }

    useEffect(()=>{
        if (!fetching){
            return;
        }
        
        let queryString = getParamsForUrl();
        let url = `${options.url}?skip=${numberOfLoadedRecords}&take=${pagination}${queryString}`;
        $api.get(url)
            .then(response => {
                if(response.status.toString().startsWith("5")){
                    console.error(`При обращениии по ${url} произошла ошибка. Статус: ${response.status}. 
                        Текст статуса: ${response.statusText}`);
                    return;
                }
                
                setRecords([...records, ...response.data]);
                allLoaded.current = response.data.length < pagination;
                setNumberOfLoadedRecords(numberOfLoadedRecords + response.data.length);
            })
            .finally(() => setFetching(false))
    }, [fetching]);
    
    return {
        setFetching,
        reset,
        records,
        allLoaded,
        fetching
    }
}