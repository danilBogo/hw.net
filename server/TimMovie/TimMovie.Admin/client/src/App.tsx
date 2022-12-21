import React from 'react';
import { Route, Routes } from "react-router-dom";
import UsersTablePage from "./components/tableWithUser/UsersTablePage";
import Login from "./components/login/Login";
import Layout from "./components/shared/Layout";
import UserProfile from "./components/userProfile/UserProfile";
import AuthProvider from "./components/auth/AuthProvider";
import RequireAuth from "./components/auth/RequireAuth";
import BannersPage from "./components/banners/BannersPage";
import FilmsPage from "./components/films/FilmsPage";
import FilmEditPage from "./components/films/FilmEditPage";
import ActorsProducersPage from "./components/actorsProduces/ActorsProducersPage";
import GenresPage from "./components/genres/GenresPage";
import SubscribesPage from "./components/subscribes/SubscribesPage";
import SubscribeEdit from "./components/subscribes/SubscribeEdit";

function App() {
    return (
        <AuthProvider>
            <Routes>
                <Route path="/login" element={<Login/>}/>
                <Route path="/" element={<RequireAuth><Layout/></RequireAuth>}>
                    <Route index element={<UsersTablePage/>}/>
                    <Route path="/banners" element={<BannersPage borderRadius={40} photoHeight={512} photoWidth={1100} />}/>
                    <Route path="/users/:id" element={<UserProfile/>}/>
                    <Route path="/films/collection" element={<FilmsPage/>}/>
                    <Route path="/films/:id" element={<FilmEditPage/>}/>
                    <Route path="/actorsProducers" element={<ActorsProducersPage/>}/>
                    <Route path="/genres" element={<GenresPage/>}/>
                    <Route path="/subscribes/collection" element={<SubscribesPage/>}/>
                    <Route path="/subscribes/:id" element={<SubscribeEdit/>}/>
                </Route>
            </Routes>
        </AuthProvider>
    );
}

export default App;
