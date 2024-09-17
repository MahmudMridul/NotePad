import { createBrowserRouter } from "react-router-dom";
import LoginRegistration from "./pages/LoginRegistration";
import Home from "./pages/Home";
import ProtectedRoute from "./components/ProtectedRoute";

export const router = createBrowserRouter([
   {
      path: "/",
      element: <LoginRegistration />,
   },
   {
      path: "/home",
      element: (
         <ProtectedRoute>
            <Home />
         </ProtectedRoute>
      ),
   },
]);
