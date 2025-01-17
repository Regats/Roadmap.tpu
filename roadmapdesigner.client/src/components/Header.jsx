import React from "react";
import { Link } from "react-router-dom";

export function Header() {
    const handleLogin = () => {
        const clientId = "Ваш_client_id"; // Укажите ваш client_id
        const redirectUri = encodeURIComponent("https://localhost:5173/callback");
        const state = encodeURIComponent("random_state_string");

        const authUrl = `https://oauth.tpu.ru/authorize?client_id=${clientId}&redirect_uri=${redirectUri}&response_type=code&state=${state}`;
        window.location.href = authUrl;
    };

    return (
        <header>
            <div className="container-header">
                <div className="header">
                    <a href="https://tpu.ru/" className="header__logo" target="_blank" rel="noopener noreferrer">
                        <img src="/accets/images/ЛОГО.svg" alt="Логотип" />
                    </a>
                    <div className="header__nav">
                        <nav className="nav">
                            <Link to="/" className="nav__link">Главная</Link>
                            <Link to="/constructor" className="nav__link">Дорожные карты</Link>
                            <button className="btn" onClick={handleLogin}>Войти</button>
                        </nav>
                    </div>
                </div>
            </div>
        </header>
    );
}

export default Header;
