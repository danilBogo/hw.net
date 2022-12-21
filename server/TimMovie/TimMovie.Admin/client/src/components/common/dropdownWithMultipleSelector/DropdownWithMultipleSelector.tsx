import React, {useState} from "react";
import dropdownItemsClasses from "./dropdownWithMultipleSelector.module.css"; 
import DropdownWithMultipleSelectorProps from "./DropdownWithMultipleSelectorProps";
import NameDto from "../../../dto/NameDto";
import DropdownWithSearch from "../dropdownWithSearch/DropdownWithSearch";


function DropdownWithMultipleSelector({values, ...props}: DropdownWithMultipleSelectorProps){
    const [selectedValuesIsChanged, setSelectedValuesIsChanged] = useState<boolean>(false);
    const [showDropdown, setShowDropdown] = useState(false);
    
    function toggleItem(value: string){
        if(values.current.has(value)){
            values.current.delete(value);
        } else {
            values.current.add(value);
        }
        setSelectedValuesIsChanged(!selectedValuesIsChanged);
        
        if(props.onChangeSelectedValues){
            props.onChangeSelectedValues();
        }
    }
    
    const parsedResultFunction = (entity: NameDto) => {
        let isSelected = values.current.has(entity.name);

        return (
            <div key={entity.id} className={dropdownItemsClasses.menuItem} onClick={() => toggleItem(entity.name)}>
                <div className={ isSelected
                    ? dropdownItemsClasses.menuItemActive
                    : ""}>{entity.name}</div>
                {isSelected &&
                    <svg xmlns="http://www.w3.org/2000/svg" width="20"
                         fill="green" className="bi bi-check-lg" viewBox="0 0 16 16">
                        <path
                            d="M12.736 3.97a.733.733 0 0 1 1.047 0c.286.289.29.756.01 1.05L7.88 12.01a.733.733 0 0 1-1.065.02L3.217 8.384a.757.757 0 0 1 0-1.06.733.733 0 0 1 1.047 0l3.052 3.093 5.4-6.425a.247.247 0 0 1 .02-.022Z"/>
                    </svg>}
            </div>
        );
    }
    
    return (
        <>
            <DropdownWithSearch
                url={props.urlRequestForEntity}
                pagination={props.pagination}
                parsedResultFunction={parsedResultFunction}
                title={props.title}
                showMenu={showDropdown}
                setShowMenu={setShowDropdown}
                onClickDropdown={props.onClickDropdownButton}
            />
        </>
    );
}

export default DropdownWithMultipleSelector;