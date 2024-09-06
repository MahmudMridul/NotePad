import { Box, Typography } from "@mui/material";
import React from "react";

export default function AppBar() {
   return (
      <Box sx={{ width: "98vw", mb: 2 }}>
         <Typography textAlign={"end"}>Name of User</Typography>
      </Box>
   );
}
