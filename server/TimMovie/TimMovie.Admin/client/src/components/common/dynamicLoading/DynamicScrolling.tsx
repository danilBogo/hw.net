import React, {useEffect, useState} from "react";
import DynamicScrollingProps from "./DynamicScrollingProps";
import Loader from "../loader/Loader";

function DynamicScrolling({paginationResult, ...props}: DynamicScrollingProps){
    const [showLoader, setShowLoader] = useState(false);
    
    function tryLoadMoreRecords(): void {
        const offsetHeight = document.body.offsetHeight;
        const innerHeight = window.innerHeight;
        const scrollY = window.scrollY;

        paginationResult.setFetching(props.amountScreenBeforeLoading * innerHeight >= offsetHeight - scrollY 
            && !paginationResult.allLoaded.current);
    }

    useEffect(() => {
        window.addEventListener("scroll", tryLoadMoreRecords);

        return function (){
            window.removeEventListener("scroll", tryLoadMoreRecords);
        }
    }, []);

    useEffect(() => {
        setShowLoader(paginationResult.fetching);
    }, [paginationResult.fetching])
    
    return (
        <div className={props.className ?? ""}>
            {props.children}
            {showLoader && <Loader/>}
        </div>
    );
}

export default DynamicScrolling;