import { useEffect } from "react";
import axios from "axios";

export function TestRequest() {
    useEffect(() => {
        // ���������� �������� ������
        axios.get("http://localhost:5192/api/test") // ������� �������� URL ������ �������
            .then(response => {
                console.log("����� �� �������:", response.data);
            })
            .catch(error => {
                console.error("������ ��� �������:", error);
            });
    }, []);

    return <div>��������� ���������� � ��������...</div>;
}
