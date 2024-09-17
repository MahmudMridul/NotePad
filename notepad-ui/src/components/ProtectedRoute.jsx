import React from "react";
import { useSelector } from "react-redux";
import { Navigate, useLocation } from "react-router-dom";

export default function ProtectedRoute({ children }) {
   const states = useSelector((store) => store.app);
   const { isAuth } = states;

   const location = useLocation();

   if (!isAuth) {
      return <Navigate to={"/"} state={{ from: location }} replace />;
   }
   return children;
}
