import { useEffect } from "react";
import axios from "axios";

export function TestRequest() {
    useEffect(() => {
        // Отправляем тестовый запрос
        axios.get("http://localhost:5192/api/test") // Укажите реальный URL вашего сервера
            .then(response => {
                console.log("Ответ от сервера:", response.data);
            })
            .catch(error => {
                console.error("Ошибка при запросе:", error);
            });
    }, []);

    return <div>Проверяем соединение с сервером...</div>;
}
