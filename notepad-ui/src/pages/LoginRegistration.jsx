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
import { useDispatch } from "react-redux";
import { signIn, signUp } from "../appSlice";
import { useNavigate } from "react-router-dom";
import Loading from "../components/Loading";

export default function LoginRegistration() {
   const dispatch = useDispatch();

   const [showPassword, setShowPassword] = useState(false);
   const [isLogin, setIsLogin] = useState(true);

   const [name, setName] = useState("");
   const [email, setEmail] = useState("");
   const [password, setPassword] = useState("");

   const navigate = useNavigate();

   function handleClickShowPassword() {
      setShowPassword(!showPassword);
   }

   function resetFields() {
      setName("");
      setEmail("");
      setPassword("");
   }

   function toggleSigninSignup() {
      resetFields();
      setIsLogin(!isLogin);
   }

   function handleName(e) {
      let v = e.target.value;
      setName(v);
   }

   function handleEmail(e) {
      let v = e.target.value;
      setEmail(v);
   }

   function handlePassword(e) {
      let v = e.target.value;
      setPassword(v);
   }

   function handleSignInSignUp() {
      if (isLogin) {
         handleSignIn();
      } else {
         handleSignUp();
      }
   }

   function handleSignIn() {
      const obj = {
         email,
         password,
      };
      dispatch(signIn(obj)).then((res) => {
         if (res.payload.isSuccess) {
            navigate("/home");
         } else {
            console.log("stay in this page");
         }
      });
   }

   function handleSignUp() {
      const obj = {
         name,
         email,
         password,
      };
      dispatch(signUp(obj));
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
         <Loading />
         <Box sx={{ p: 2, display: "flex", flexDirection: "column" }}>
            <TextField
               sx={{ mb: 3, width: 300, display: isLogin ? "none" : "inherit" }}
               required
               label="Name"
               type="text"
               size="small"
               value={name}
               onChange={handleName}
            />

            <TextField
               sx={{ mb: 3, width: 300 }}
               required
               label="Email"
               type="email"
               size="small"
               value={email}
               onChange={handleEmail}
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
                  value={password}
                  onChange={handlePassword}
               />
            </FormControl>

            <Button
               sx={{ width: 300, mb: 3 }}
               variant="outlined"
               onClick={handleSignInSignUp}
            >
               {isLogin ? "Sign In" : "Sign Up"}
            </Button>

            <Typography
               sx={{ cursor: "pointer" }}
               align="right"
               color="info"
               onClick={toggleSigninSignup}
            >
               {isLogin ? "Create an account" : "Sign In"}
            </Typography>
         </Box>
      </Box>
   );
}
