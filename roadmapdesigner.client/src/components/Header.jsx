import { Link } from "react-router";
export function Header(){
    return(
            <header>
                <div className="container-header">
                    <div className="header">
                        <a href="https://tpu.ru/" className="header__logo" target="_blank" >
                            <img src='/accets/images/ЛОГО.svg' alt="" />
                        </a>
                        <div className="header__nav">
                            <nav className="nav">
                                <Link to="/" className="nav__link">Главная</Link>
                                <Link href="#" className="nav__link">О нас</Link>
                                <Link to="/constructor" className="nav__link">Дорожные карты</Link>
                                <Link to='profile' className="btn">Войти</Link>
                            </nav>
                        </div>
                    </div>
                </div>
            </header>
    );
}