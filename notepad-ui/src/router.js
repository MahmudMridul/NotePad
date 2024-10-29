import { createBrowserRouter } from "react-router-dom";
import LoginRegistration from "./pages/LoginRegistration";
import Home from "./pages/Home";
import ProtectedRoute from "./components/ProtectedRoute";
import Edit from "./pages/Edit";

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
   {
      path: "/edit/:noteId",
      element: (
         <ProtectedRoute>
            <Edit />
         </ProtectedRoute>
      ),
   },
]);
