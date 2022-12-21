import React, {LegacyRef, MouseEventHandler} from 'react';

interface ISearchProps{
    label: string;
    onClickSearchBtn: MouseEventHandler<HTMLButtonElement>;
    className: string;
}

const Search = React.forwardRef( ({label, onClickSearchBtn, className}: ISearchProps, ref: LegacyRef<HTMLInputElement>) => {
    return (
        <div className="form-group col-md-3">
            <label htmlFor="searchBar" className="mb-2">{label}</label>
            <div className="d-flex">
                <input ref={ref} type="text" className={className} id="searchBar" placeholder=""/>
                <button className="btn btn-outline-info ms-3" onClick={onClickSearchBtn}>Поиск</button>
            </div>
        </div>
    );
});

export default  Search;