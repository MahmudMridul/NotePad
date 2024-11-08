import { createBrowserRouter } from "react-router-dom";
import Home from "./pages/Home";
import ProtectedRoute from "./components/ProtectedRoute";
import Edit from "./pages/Edit";
import Create from "./pages/Create";
import Signin from "./pages/Signin";
import Signup from "./pages/Signup";

export const router = createBrowserRouter([
   {
      path: "/",
      element: <Signin />,
   },
   {
      path: "/signup",
      element: <Signup />,
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
   {
      path: "/new",
      element: (
         <ProtectedRoute>
            <Create />
         </ProtectedRoute>
      ),
   },
]);
