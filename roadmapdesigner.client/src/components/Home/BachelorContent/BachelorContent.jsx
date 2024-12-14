import './style.css'
import { Link } from "react-router";
import { useContext} from 'react';
import { Contex } from '../../../Context';

export function BachelorConten(){

    const { setId, setTitle } = useContext(Contex)

    const bd = [
        {
            id:1,
            name: "Программная инженерия",
            numberDirection: "09.03.04"
        },
        {
            id: 2,
            name: "Прикладная информатика",
            numberDirection: "09.03.04"
        }
    ]

    const takeAttributes = (event) => {
        setTitle(`${bd[event.target.id - 1].numberDirection} ${bd[event.target.id - 1].name}`)
        setId(event.target.id)
    }


    return(
        <>
            <div className="container-content__direction">
                <div className="column_direction">
                    <div className="block_direction">
                        <div className="titile_direction">КОМПЬЮТЕРНЫЕ НАУКИ</div>
                        <Link href="#!" className="directions">Прикладная математика и информатика</Link>
                        <Link href="#!" className="directions">Информатика и вычислительная техника</Link>
                        <Link to="#!" className="directions">Информационные системы и технологии</Link>
                        <Link to={`/${bd[1].id}`} id={bd[1].id} className="directions" onClick={takeAttributes}>{bd[1].name}</Link>
                        <Link to={`/${bd[0].id}`} id={bd[0].id} className="directions" onClick={takeAttributes}>{bd[0].name}</Link>
                    </div>
                    <div className="block_direction">
                        <div className="titile_direction">ЭНЕРГЕТИЧЕСКИЕ СИСТЕМЫ</div>
                        <Link href="#!" className="directions">Теплоэнергетика и теплотехника</Link>
                        <Link href="#!" className="directions">Электроэнергетика и электротехника</Link>
                    </div>
                    <div className="block_direction">
                        <div className="titile_direction">ГЕОНАУКИ</div>
                        <Link href="#!" className="directions">Нефтегазовое дело</Link>
                        <Link href="#!" className="directions">Землеустройство и кадастры</Link>
                        <Link href="#!" className="directions">Прикладная геология</Link>
                        <Link href="#!" className="directions">Технология геологической разведки</Link>
                    </div>
                    <div className="block_direction">
                        <div className="titile_direction">МАТЕРИАЛОВЕДЕНИЕ</div>
                        <Link href="#!" className="directions">Материаловедение<br />и технологии материалов </Link>
                        <Link href="#!" className="directions">Металлургия</Link>
                    </div>
                </div>
                <div className="column_direction">
                    <div className="block_direction">
                        <div className="titile_direction">ФИЗ. НАУКИ И ТЕХНОЛОГИИ</div>
                        <Link href="#!" className="directions">Физика</Link>
                        <Link href="#!" className="directions">Электроника и наноэлектроника</Link>
                        <Link href="#!" className="directions">Приборостроение</Link>
                        <Link href="#!" className="directions">Оптотехника</Link>
                        <Link href="#!" className="directions">Ядерные физика и технологии</Link>
                    </div>
                    <div className="block_direction">
                        <div className="titile_direction">МАШИНОСТРОЕНИЕ И АВТОМАТИЗАЦИЯ</div>
                        <Link href="#!" className="directions">Машиностроение</Link>
                        <Link href="#!" className="directions">Технологические машины <br />и оборудование</Link>
                        <Link href="#!" className="directions">Автоматизация технологических <br />процессов и производств</Link>
                        <Link href="#!" className="directions">Мехатроника и робототехника</Link>
                    </div>
                </div>
                <div className="column_direction">
                    <div className="block_direction">
                        <div className="titile_direction">ЯДЕРНЫЕ ТЕХНОЛОГИИ</div>
                        <Link href="#!" className="directions">Атомные станции: проектирование,<br />эксплуатация и инжиниринг</Link>
                        <Link href="#!" className="directions">Электроника и автоматика <br />физических установок</Link>
                    </div>
                    <div className="block_direction">
                        <div className="titile_direction">ЭКОЛОГИЧЕСКИЕ НАУКИ</div>
                        <Link href="#!" className="directions">Экология и природопользование</Link>
                        <Link href="#!" className="directions">Химическая технология</Link>
                        <Link href="#!" className="directions">Биотехнология</Link>
                    </div>
                    <div className="block_direction">
                        <div className="titile_direction">УПРАВЛЕНИЕ И ЭКОНОМИКА</div>
                        <Link href="#!" className="directions">Управление качеством</Link>
                        <Link href="#!" className="directions">Инноватика</Link>
                        <Link href="#!" className="directions">Экономика</Link>
                        <Link href="#!" className="directions">Менеджмент</Link>
                    </div>
                </div>
            </div>
        </>
    )
}