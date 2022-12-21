import requiredClasses from "../css/required.module.css";
import React from "react";

function RequiredFieldIcon(){
    return (
        <span className={requiredClasses.requiredField}>*</span>
    );
}

export default RequiredFieldIcon