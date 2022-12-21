import React, {createContext} from "react";
import jwtDecode from "jwt-decode";
import IAuthContext from "./IAuthContext";

export const AuthContext = createContext<IAuthContext | null>(null);

function AuthProvider({children}: any){
    const login = (token: string): void => {
        localStorage.setItem(`${process.env.REACT_APP_ACCESS_TOKEN_KEY_NAME}`, token);
    }

    const logout = (): void => {
        localStorage.removeItem(`${process.env.REACT_APP_ACCESS_TOKEN_KEY_NAME}`);
    }

    function isAdmin(token: string | null): boolean{
        if(token === null)
            return false;
        const decodedToken: any = jwtDecode(token);
        const role = decodedToken[`${process.env.REACT_APP_CLAIM_ROLE}`];
        if(role === undefined || role == null)
            return false;
        return role.includes('admin');
    }

    function isAdminAuth(): boolean{
        const token = localStorage.getItem(`${process.env.REACT_APP_ACCESS_TOKEN_KEY_NAME}`)
        if(token === undefined)
            return false;
        return isAdmin(token);
    }
    
    const value: IAuthContext = {login, logout, isAdmin, isAdminAuth};
    
    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>);
}

export default AuthProvider;