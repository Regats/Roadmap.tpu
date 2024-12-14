export function Footer(){

    let date = new Date().getFullYear();

    return(
        <>
            <div className="container-footer">
                <div className="container-footer_content">
                    <div className="div">
                        <div className="footer_logo">
                            <img src='/accets/images/ЛОГО.svg' alt="" />
                        </div>
                        <div className="footer_content_name">© {date} Национальный исследовательский <br />Томский политехнический университет</div>
                        <div className="footer_label">Приемная комиссия ТПУ</div>
                        <img className="rect"src="/accets/image/Rect.svg" alt="" />
                        <div className="footer_contact">Главный корпус ТПУ, офис 131В, г. Томск, проспект<br />Ленина, 30</div>
                        <div className="footer_contact">8-800-350-28-09 (для граждан РФ)</div>
                        <div className="footer_contact">+7 (3822) 60-99-09 (для иностранных граждан)</div>
                        <div className="footer_contact"><u>abiturient@tpu.ru</u></div>
                    </div>
                    <div className="footer_roadmap">Roadmap.tpu</div>
                </div>
            </div>
        </>
    )

}