import s from "./style.module.css";
import { Link } from "react-router";

export function SelectType({title}) {

  return (
    <>
      <div className={s.container}>
        <div className={s.content}>
            <div className={s.btns}>
                <div className={s.row}>
                <img className={s.arrow} src="/accets/images/arrow.svg" alt="" />
                <Link to="/" className={s.back}>Вернуться назад</Link>
            </div>
            <div className={s.row}>
                <button className={s.btn}>
                    <img
                        className={s.share}
                        src="/accets/images/Share.svg"
                        alt=""
                    />
                    <h1>Поделиться</h1>
                </button>
            </div>
            </div>
            <div className={s.title}>{title}</div>
            <div className={s.paragraph}>
                Направление подготовки включает в себя одну или несколько
                образовательных программ, которые кардинально отличаются друг от
                друга.<br />В свою очередь основная образовательная программа содержит
                специализацию, <br />в зависимости от нее учебный план тоже меняется, но
                менее существенно.<br />Помимо этого влияние на отображаемый учебный план
                оказывает форма обучения.
            </div>
            <div className={s.selectNameProgramm}>Основная образовательная программа</div>
            <select name="selectedProgramm">
                <option value="">Выбор из списка</option>
                <option value="apple">Apple</option>
                <option value="banana">Banana</option>
                <option value="orange">Orange</option>
            </select>
            <div className={s.selectNameProgramm}>Форма обучения</div>
            <select name="selectedProgramm">
                <option value="">Выбор из списка</option>
                <option value="apple">Очная</option>
                <option value="banana">Очно-заочная</option>
                <option value="orange">Заочная</option>
            </select>
            <button className={s.btnFind}>
                <h1>Найти</h1>
                <img src="/accets/images/arrowRightCircle.svg" alt="" />
            </button>
        </div>
      </div>
    </>
  );
}