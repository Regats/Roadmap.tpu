import { Header } from './components/Header'
import {React, useState, useEffect} from 'react'
import './styles/main.css'
import { Footer } from './components/Footer'
import { Routes, Route, Router, useLocation} from "react-router";
import { OwnBody } from './components/Home/OwnBody'
import { SelectType } from './components/SelectPage/SelectType';
import { Contex } from './Context';
import {Constructor} from './components/Constructor/Constructor'

export function App(){

  const [id, setId] = useState(() => localStorage.getItem('id') || '');
  const [title, setTitle] = useState(() => localStorage.getItem('title') || '');

  useEffect(() => {
    localStorage.setItem('id', id);
    localStorage.setItem('title', title);
  }, [id, title]);

  const {pathname} = useLocation()

  useEffect(() => {
    window.scrollTo({top: 0, behavior: "smooth"} )
  }, [pathname])

  return(
    <Contex.Provider value={{ id, setId, title, setTitle }}>
      <>
        <Header/>          
          <Routes>
            <Route path="/" element={<OwnBody/>} />
            <Route path={`/${id}`} element={<SelectType title={title}/>}/>
            <Route path='/constructor' element={<Constructor/>}/>
          </Routes>        
        <Footer/>
      </>
    </Contex.Provider>
  )
}