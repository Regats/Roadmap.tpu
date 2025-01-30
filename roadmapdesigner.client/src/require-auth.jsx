import { useAuth } from "./auth-context";
import { Navigate } from "react-router-dom";

export function RequireAuth({ children }) {
    const { user, isLoading } = useAuth();

    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (!user) {
        return <Navigate to="/" replace />;
    }

    return children;
}