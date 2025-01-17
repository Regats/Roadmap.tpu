import { useState } from "react"
import { BachelorConten } from "./BachelorContent/BachelorContent";

export function OwnBody() {

    const [activeButton, setActiveButton] = useState(1);
    const [hoveredButton, setHoveredButton] = useState(null); // Новое состояние для отслеживания наведения

    const buttonStyle = (buttonId) => ({
        backgroundColor: activeButton === buttonId ? 'black' : (hoveredButton === buttonId ? 'rgba(98, 98, 98, 0.75)' : 'white'),
        color: (activeButton === buttonId) ? 'white' : (hoveredButton === buttonId ? 'white' : 'black'),
    });

    const handleClick = (buttonId) => {
        setActiveButton(buttonId == activeButton? null : buttonId)
    }

    const handleMouseEnter = (buttonId) => {
        setHoveredButton(buttonId); r

    };

    const handleMouseLeave = () => {
        setHoveredButton(null);
    };

    return (
        <>
            <div className="container_body">
                <div className="conteiner-content_name">
                    <div className="content-name_behaind"><h1>ROADMAP.TPU</h1></div>
                    <div className="content-name">ROADMAP.TPU</div>
                </div>
                <div className="container_boards">
                    <div className="first_board_position">
                        <div>
                            <div className="first_board board">
                                <div className="yellow_text">Добро пожаловать</div>
                                <div className="content_text">Roadmap.tpu — сервис НИ ТПУ, на котором ты можешь изучить образовательные программы нашего университета в удобном и понятном виде</div>
                            </div>
                            <div className="lower_part"></div>
                        </div>
                        <img className="first_line" src="/accets/images/Первая стрелка.svg" alt="" />
                    </div>
                    <div className="second_board_position">
                        <img className="second_line" src="/accets/images/Вторая стрелка.svg" alt="" />
                        <div>
                            <div className="second_board board">
                                <div className="yellow_text">Посмотри чему мы учим</div>
                                <div className="content_text">Внушительный список программ подготовки инженерной элиты России и мира. Помни, что у некоторых направлений больше, чем одна образовательная программа</div>
                            </div>
                            <div className="lower_part"></div>
                            <div className="step_1">Шаг 1</div>
                        </div>
                    </div>
                    <div className="third_board_position">
                        <div>
                            <div className="third_board board">
                                <div className="yellow_text">Изучи дорожную карту</div>
                                <div className="content_text">В которой нагляно отображается вся образовательная программа целого направления подготовки с предметами, описаниями и ссылками</div>
                            </div>
                            <div className="lower_part"></div>
                            <div className="step_2">Шаг 2</div>
                        </div>
                        <img className="first_line" src="/accets/images/Первая стрелка.svg" alt="" />
                    </div>
                    <div className="fourth_board_position">
                        <div>
                            <div className="fourth_board board">
                                <div className="yellow_text">Выбери своё будущее</div>
                                <div className="content_text">Изучив напрвление с помощью дорожной карты, выбери наиболее подходящее для тебя</div>
                            </div>
                            <div className="lower_part"></div>
                            <div className="step_3">Шаг 3</div>
                        </div>
                    </div>
                </div>
            </div>
            <div className="container_own_body">
                <div className="container_direction">
                    <div className="table_content">Направления подготовки</div>
                    <div className="step_education">
                        <button className="btn_bachelor"
                            onClick={() => handleClick(1)} 
                            onMouseEnter={() => handleMouseEnter(1)}
                            onMouseLeave={handleMouseLeave}
                            style={buttonStyle(1)}>БАКАЛАВРИАТ И СПЕЦИАЛИТЕТ</button>
                        <button className="btn_magistracy"
                            onClick={() => handleClick(2)} 
                            onMouseEnter={() => handleMouseEnter(2)}
                            onMouseLeave={handleMouseLeave}
                            style={buttonStyle(2)}>МАГИСТРАТУРА</button>
                        <button className="btn_graduate"
                            onClick={() => handleClick(3)} 
                            onMouseEnter={() => handleMouseEnter(3)}
                            onMouseLeave={handleMouseLeave}
                            style={buttonStyle(3)}>АСПИРАНТУРА</button>
                    </div>
                    {activeButton === 1 && <BachelorConten />}
                </div>
            </div>
        </>
    )
}