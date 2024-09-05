import { Visibility, VisibilityOff } from "@mui/icons-material";
import {
   Box,
   Button,
   FormControl,
   IconButton,
   InputAdornment,
   InputLabel,
   OutlinedInput,
   TextField,
   Typography,
} from "@mui/material";
import React, { useState } from "react";

export default function LoginRegistration() {
   const [showPassword, setShowPassword] = useState(false);
   const [isLogin, setIsLogin] = useState(true);

   function handleClickShowPassword() {
      setShowPassword(!showPassword);
   }

   function setToSignUpMode() {
      setIsLogin(!isLogin);
   }

   return (
      <Box
         sx={{
            height: "97vh",
            width: "97vw",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
         }}
      >
         <Box sx={{ p: 2, display: "flex", flexDirection: "column" }}>
            <TextField
               sx={{ mb: 3, width: 300, display: isLogin ? "none" : "inherit" }}
               required
               label="Name"
               type="text"
               size="small"
            />

            <TextField
               sx={{ mb: 3, width: 300 }}
               required
               label="Email"
               type="email"
               size="small"
            />

            <FormControl size="small" variant="outlined" required>
               <InputLabel htmlFor="outlined-adornment-password">
                  Password
               </InputLabel>
               <OutlinedInput
                  sx={{ width: 300, mb: 3 }}
                  id="outlined-adornment-password"
                  type={showPassword ? "text" : "password"}
                  endAdornment={
                     <InputAdornment position="end">
                        <IconButton
                           aria-label="toggle password visibility"
                           onClick={handleClickShowPassword}
                           edge="end"
                        >
                           {showPassword ? <VisibilityOff /> : <Visibility />}
                        </IconButton>
                     </InputAdornment>
                  }
                  label="Password"
               />
            </FormControl>

            <Button sx={{ width: 300, mb: 3 }} variant="outlined">
               {isLogin ? "Sign In" : "Sign Up"}
            </Button>

            <Typography
               sx={{ cursor: "pointer" }}
               align="right"
               color="info"
               onClick={setToSignUpMode}
            >
               {isLogin ? "Create an account" : "Sign In"}
            </Typography>
         </Box>
      </Box>
   );
}
