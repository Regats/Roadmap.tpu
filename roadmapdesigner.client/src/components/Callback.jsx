import React, { useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Callback = () => {
    const navigate = useNavigate();

    useEffect(() => {
        const params = new URLSearchParams(window.location.search);
        const code = params.get("code");

        if (code) {
            axios
                .post("http://localhost:5192/api/auth/token", { code })
                .then(response => {
                    console.log("Token:", response.data);
                    localStorage.setItem("auth_token", response.data.access_token);
                    navigate("/profile"); // �������������� ������������ �� �������
                })
                .catch(error => {
                    console.error("������ ��� ������ ���� �� �����:", error);
                });
        }
    }, [navigate]);

    return <div>�����������...</div>;
};

export default Callback;
