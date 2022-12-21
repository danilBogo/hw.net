import React, {useState} from "react";
import dropdownItemsClasses from "./DropdownSearchOneValue.module.css"
import NameDto from "../../../dto/NameDto";
import DropdownWithSearch from "../dropdownWithSearch/DropdownWithSearch";
import DropdownSearchOneValueProps from "./DropdownSearchOneValueProps";

function DropdownSearchOneValue({value, ...props}: DropdownSearchOneValueProps){
    const [showDropdown, setShowDropdown] = useState(false);
    
    function toggleItem(name: string){
        setShowDropdown(false);
        props.setValue(name);
    }

    const parsedResultFunction = (entity: NameDto) => {
        let isSelected = value === entity.name;

        return (
            <div key={entity.id} className={dropdownItemsClasses.menuItem} onClick={() => toggleItem(entity.name)}>
                <div className={ isSelected
                    ? dropdownItemsClasses.menuItemActive
                    : ""}>{entity.name}</div>
            </div>
        );
    }

    return (
        <DropdownWithSearch
            url={props.urlRequestForEntity}
            pagination={props.pagination}
            parsedResultFunction={parsedResultFunction}
            title={props.title}
            setShowMenu={setShowDropdown}
            showMenu={showDropdown}
            onClickDropdown={props.onClickDropdownButton}
        />
    );
}

export default DropdownSearchOneValue;