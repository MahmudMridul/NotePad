import { Button, Stack, TextField } from "@mui/material";
import React, { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useLocation, useNavigate } from "react-router-dom";
import { editNote, getNotesForUser } from "../appSlice";

export default function Edit() {
   const location = useLocation();
   const navigate = useNavigate();
   const dispatch = useDispatch();

   const { id, title, description } = location.state;

   const [eTitle, seteTitle] = useState(title);
   const [eDesc, seteDesc] = useState(description);

   const state = useSelector((store) => store.app);
   const { loggedInUserEmail } = state;

   function goBack() {
      navigate("/home");
   }

   function saveChanges() {
      const payload = {
         id,
         title: eTitle,
         description: eDesc,
      };
      dispatch(editNote(payload)).then(() => {
         goBack();
         dispatch(getNotesForUser(loggedInUserEmail));
      });
   }

   function handleTitle(e) {
      const val = e.target.value;
      seteTitle(val);
   }

   function handleDesc(e) {
      const val = e.target.value;
      seteDesc(val);
   }

   return (
      <Stack sx={{ width: "30%" }}>
         <TextField
            variant="standard"
            label="Title"
            value={eTitle}
            onChange={handleTitle}
         />
         <TextField
            variant="standard"
            label="Description"
            value={eDesc}
            onChange={handleDesc}
         />
         <Button
            color="success"
            variant="contained"
            sx={{ textTransform: "none" }}
            onClick={saveChanges}
         >
            Save
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
