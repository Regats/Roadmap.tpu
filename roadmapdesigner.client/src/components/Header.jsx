import React from "react";
import { Link } from "react-router-dom";
import { useAuth } from '../auth-context';

export function Header() {
    const { user, login, logout, isLoading } = useAuth();

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
                            {isLoading ? null : user ? (
                                <span
                                    style={{ cursor: 'pointer' }}
                                    className="btn"
                                    onClick={logout}
                                >
                                    {user.name}
                                </span>
                            ) : (
                                <button className="btn" onClick={login}>Войти</button>
                            )}

                        </nav>
                    </div>
                </div>
            </div>
        </header>
    );
}

export default Header;