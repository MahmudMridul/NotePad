import React, { useState } from "react";
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
import Loading from "../components/Loading";
import { useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { signIn } from "../appSlice";

export default function Signin() {
   const navigate = useNavigate();
   const dispatch = useDispatch();

   const [showPassword, setShowPassword] = useState(false);

   const [email, setEmail] = useState("");
   const [password, setPassword] = useState("");

   function handleClickShowPassword() {
      setShowPassword(!showPassword);
   }

   function handleEmail(e) {
      let v = e.target.value;
      // validateEmail(v);
      setEmail(v);
   }

   function handlePassword(e) {
      let v = e.target.value;
      // validatePassword(v);
      setPassword(v);
   }

   function gotoSignUp() {
      navigate("/signup");
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
               sx={{ mb: 3, width: 300 }}
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
                  sx={{ width: 300, mb: 3 }}
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

            <Button
               sx={{ width: 300, mb: 3 }}
               variant="outlined"
               onClick={handleSignIn}
            >
               Sign In
            </Button>

            <Typography
               sx={{ cursor: "pointer" }}
               align="right"
               color="info"
               onClick={gotoSignUp}
            >
               Create an account
            </Typography>
         </Box>
      </Box>
   );
}
