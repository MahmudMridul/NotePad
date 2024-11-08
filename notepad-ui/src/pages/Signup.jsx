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
import { useNavigate } from "react-router-dom";
import Loading from "../components/Loading";
import {
   containsBothCases,
   containsNumber,
   containsSpecialCharacter,
   isEightChars,
   isValidEmail,
} from "../utils/functions";
import { signUp } from "../appSlice";

export default function Signup() {
   const dispatch = useDispatch();

   const [showPassword, setShowPassword] = useState(false);

   const [name, setName] = useState("");
   const [email, setEmail] = useState("");
   const [password, setPassword] = useState("");

   const [emailError, setEmailError] = useState(false);
   const [passError, setPassError] = useState(false);

   const navigate = useNavigate();

   function handleClickShowPassword() {
      setShowPassword(!showPassword);
   }

   function resetFields() {
      setName("");
      setEmail("");
      setPassword("");
   }

   function handleName(e) {
      let v = e.target.value;
      setName(v);
   }

   function handleEmail(e) {
      let v = e.target.value;
      validateEmail(v);
      setEmail(v);
   }

   function validateEmail(email) {
      if (email.length === 0) {
         setEmailError(false);
      } else if (email.length >= 6 && !isValidEmail(email)) {
         setEmailError(true);
      } else if (email.length >= 6 && isValidEmail(email)) {
         setEmailError(false);
      }
   }

   function handlePassword(e) {
      let v = e.target.value;
      validatePassword(v);
      setPassword(v);
   }

   function validatePassword(pass) {
      if (pass.length === 0) {
         setPassError(false);
         return;
      }

      let notValid =
         pass.length > 0 &&
         (!isEightChars(pass) ||
            !containsBothCases(pass) ||
            !containsNumber(pass) ||
            !containsSpecialCharacter(pass));

      console.log(notValid);

      if (notValid) {
         setPassError(true);
      } else {
         setPassError(false);
      }
   }

   function handleSignUp() {
      const obj = {
         name,
         email,
         password,
      };
      dispatch(signUp(obj));
   }

   function gotoSignin() {
      navigate("/");
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
               sx={{ mb: 3, width: 300, display: "inherit" }}
               required
               label="Name"
               type="text"
               size="small"
               value={name}
               onChange={handleName}
            />

            <TextField
               sx={{ mb: 3, width: 300 }}
               error={emailError}
               required
               label="Email"
               type="email"
               size="small"
               value={email}
               onChange={handleEmail}
            />

            <FormControl size="small" variant="outlined" required>
               <InputLabel>Password</InputLabel>
               <OutlinedInput
                  sx={{ width: 300, mb: 0 }}
                  error={passError}
                  type={showPassword ? "text" : "password"}
                  endAdornment={
                     <InputAdornment position="end">
                        <IconButton
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

            <Typography sx={{ mt: 1 }} variant="caption" color="info">
               Password should have
            </Typography>
            <Typography
               variant="caption"
               color={
                  password.length > 0 && isEightChars(password)
                     ? "success"
                     : "warning"
               }
            >
               at least 8 characters
            </Typography>
            <Typography
               variant="caption"
               color={containsBothCases(password) ? "success" : "warning"}
            >
               capital and small letters
            </Typography>
            <Typography
               variant="caption"
               color={containsNumber(password) ? "success" : "warning"}
            >
               at least 1 numeric character
            </Typography>
            <Typography
               sx={{ mb: 4 }}
               variant="caption"
               color={
                  containsSpecialCharacter(password) ? "success" : "warning"
               }
            >
               at least 1 special character
            </Typography>

            <Button
               sx={{ width: 300, mb: 3 }}
               variant="outlined"
               onClick={handleSignUp}
            >
               Sign Up
            </Button>

            <Typography
               sx={{ cursor: "pointer" }}
               align="right"
               color="info"
               onClick={gotoSignin}
            >
               Sign In
            </Typography>
         </Box>
      </Box>
   );
}
