import { Header } from './components/Header'
import { React, useState, useEffect } from 'react'
import './styles/main.css'
import { Footer } from './components/Footer'
import { OwnBody } from './components/Home/OwnBody'
import { SelectType } from './components/SelectPage/SelectType';
import { Contex } from './Context';
import { Constructor } from './components/Constructor/Constructor'
import { BrowserRouter as Router, Route, Routes, useLocation } from "react-router-dom";
import Callback from "./components/Callback";
import { AuthProvider } from "./auth-context";
import { RequireAuth } from './require-auth';

export function App() {
    const [id, setId] = useState(() => localStorage.getItem('id') || '');
    const [title, setTitle] = useState(() => localStorage.getItem('title') || '');

    useEffect(() => {
        localStorage.setItem('id', id);
        localStorage.setItem('title', title);
    }, [id, title]);

    const { pathname } = useLocation()

    useEffect(() => {
        window.scrollTo({ top: 0, behavior: "smooth" })
    }, [pathname])

    return (
        <AuthProvider>
            <Contex.Provider value={{ id, setId, title, setTitle }}>
                <>
                    <Header />
                    <Routes>
                        <Route path="/callback" element={<Callback />} />
                        <Route path="/" element={<OwnBody />} />
                        <Route path={`/${id}`} element={<RequireAuth><SelectType title={title} /></RequireAuth>} />
                        <Route path='/constructor' element={<RequireAuth><Constructor /></RequireAuth>} />
                    </Routes>
                    <Footer />
                </>
            </Contex.Provider>
        </AuthProvider>
    )
}