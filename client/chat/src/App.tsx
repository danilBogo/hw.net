import React from 'react';
import {BrowserRouter, Route, Routes} from "react-router-dom";
import './App.css';
import Message from "./components/Message";

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/chat" element={<Message/>}/>
            </Routes>
        </BrowserRouter>
        )
}

export default App;
