import { Button, Stack, TextField } from "@mui/material";
import React, { useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";

export default function Create() {
   const navigate = useNavigate();
   const dispatch = useDispatch();

   const [cTitle, seteTitle] = useState("");
   const [cDesc, seteDesc] = useState("");

   function handleTitle(e) {
      const val = e.target.value;
      seteTitle(val);
   }

   function handleDesc(e) {
      const val = e.target.value;
      seteDesc(val);
   }

   function goBack() {
      navigate("/home");
   }

   function createNote() {}

   return (
      <Stack sx={{ width: "30%" }}>
         <TextField
            variant="standard"
            label="Title"
            value={cTitle}
            onChange={handleTitle}
         />
         <TextField
            variant="standard"
            label="Description"
            value={cDesc}
            onChange={handleDesc}
         />
         <Button
            color="success"
            variant="contained"
            sx={{ textTransform: "none" }}
            onClick={createNote}
         >
            Create
         </Button>
         <Button
            color="primary"
            variant="contained"
            sx={{ textTransform: "none" }}
            onClick={goBack}
         >
            Cancel
         </Button>
      </Stack>
   );
}
