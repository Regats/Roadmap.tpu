import React, { createContext, useState, useContext, useEffect } from 'react';
import axios from 'axios';

const AuthContext = createContext();

export function AuthProvider({ children }) {
    const [user, setUser] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const checkAuth = async () => {
            setIsLoading(true)
            try {
                const response = await axios.get("http://localhost:5192/api/users/me",
                    {
                        withCredentials: true
                    });

                if (response.status === 200) {
                    setUser(response.data);
                }
                else {
                    setUser(null)
                }

            }
            catch (error) {
                setUser(null)
                console.error("Ошибка при получении данных:", error);
            }
            finally {
                setIsLoading(false);
            }
        }
        checkAuth();
    }, []);

    const login = () => {
        window.location.href = 'http://localhost:5192/api/auth/login';
    };


    const logout = async () => {
        try {
            setUser(null);
            await axios.get("http://localhost:5192/api/auth/logout",
                {
                    withCredentials: true
                });
            window.location.href = '/'
        }
        catch (error) {
            console.error("Ошибка при выходе", error);
        }
    };
    const value = {
        user,
        isLoading,
        login,
        logout
    };
    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
export const useAuth = () => {
    return useContext(AuthContext);
};