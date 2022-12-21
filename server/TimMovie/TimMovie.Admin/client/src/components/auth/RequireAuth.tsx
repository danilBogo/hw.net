import React from "react"
import {Navigate, useLocation} from "react-router-dom";
import {useAuth} from "../../hook/useAuth";

function RequireAuth({children}: any){
    const location = useLocation();
    const auth = useAuth();
    
    if (!auth?.isAdminAuth()){
        return <Navigate to={'/login'} state={{from: location}}/>
    }
    
    return children;
}

export default RequireAuth;