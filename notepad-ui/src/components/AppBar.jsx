import { Box, Typography } from "@mui/material";
import React from "react";
import { useSelector } from "react-redux";

export default function AppBar() {
   const states = useSelector((store) => store.app);
   const { loggedInUserName } = states;
   return (
      <Box sx={{ width: "98vw", mb: 2 }}>
         <Typography textAlign={"end"}>{loggedInUserName}</Typography>
      </Box>
   );
}
