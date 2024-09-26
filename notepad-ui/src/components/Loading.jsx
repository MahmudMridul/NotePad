import React from "react";
import { Backdrop, CircularProgress } from "@mui/material";
import { useSelector } from "react-redux";

export default function Loading() {
   const loading = useSelector((store) => store.app.isLoading);
   return (
      <Backdrop
         sx={(theme) => ({ zIndex: theme.zIndex.drawer + 1 })}
         open={loading}
      >
         <CircularProgress color="primary" />
      </Backdrop>
   );
}
