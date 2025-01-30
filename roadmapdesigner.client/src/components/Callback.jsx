import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";

const Callback = () => {
    const navigate = useNavigate();

    useEffect(() => {
        navigate("/");
    }, [navigate]);

    return <div>Авторизация...</div>;
};

export default Callback;
