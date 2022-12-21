import {Outlet, NavLink} from "react-router-dom";
import React from "react";
import classes from "./layout.module.css"
import Logout from "../logout/Logout";
import {classNameConcat} from "../../common/classNameConcat";

function Layout(){
    const activeLink = classNameConcat(classes.navLink, classes.active);
    const activateLink = ({isActive}: any) => isActive ? activeLink : classes.navLink;
    
    return (
        <>
            <header>
                <div className="container-xl">
                    <div className={classes.navContainer}>
                        <div className="d-flex justify-content-between align-items-center h-100">
                            <div className="justify-content-between">
                                <NavLink className={activateLink} to="/" >Пользователи</NavLink>
                                <NavLink className={activateLink} to="/banners">Баннеры</NavLink>
                                <NavLink className={activateLink} to="/films/collection">Фильмы</NavLink>
                                <NavLink className={activateLink} to="/actorsProducers">Актеры и режиссеры</NavLink>
                                <NavLink className={activateLink} to="/genres">Жанры</NavLink>
                                <NavLink className={activateLink} to="/subscribes/collection">Подписки</NavLink>
                            </div>
                            <div><Logout/></div>
                        </div>
                    </div>
                </div>
            </header>
            <div className="container-xl">
                <Outlet/>
            </div>
        </>
    );
}

export default Layout;