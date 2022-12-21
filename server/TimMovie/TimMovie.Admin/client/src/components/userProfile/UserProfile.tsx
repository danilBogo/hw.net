import React from "react";
import {useParams} from "react-router-dom"
import UserInfo from "./UserInfo";


function UserProfile() {
    let {id: userId} = useParams();
    
    return (
        <div>
            <UserInfo userId={userId!}/>
        </div>
    );
}

export default UserProfile;