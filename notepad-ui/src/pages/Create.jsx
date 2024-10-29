import { Description } from "@mui/icons-material";
import { Button, Stack, TextField } from "@mui/material";
import React, { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { createNote, getNotesForUser } from "../appSlice";

export default function Create() {
   const navigate = useNavigate();
   const dispatch = useDispatch();

   const state = useSelector((store) => store.app);
   const { loggedInUserId, loggedInUserEmail } = state;

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

   function createNew() {
      const payload = {
         userId: loggedInUserId,
         title: cTitle,
         description: cDesc,
      };
      dispatch(createNote(payload)).then(() => {
         goBack();
         dispatch(getNotesForUser(loggedInUserEmail));
      });
   }

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
            onClick={createNew}
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
