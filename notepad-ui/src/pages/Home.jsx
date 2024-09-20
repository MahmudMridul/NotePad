import { Box } from "@mui/material";
import React, { useEffect } from "react";
import AppBar from "../components/AppBar";
import { useDispatch, useSelector } from "react-redux";
import { getNotesForUser } from "../appSlice";
import NoteList from "../components/NoteList";

export default function Home() {
   const dispatch = useDispatch();
   const states = useSelector((store) => store.app);
   const { loggedInUserEmail } = states;

   useEffect(() => {
      dispatch(getNotesForUser(loggedInUserEmail));
   }, []);

   return (
      <Box>
         <AppBar />
         <NoteList />
      </Box>
   );
}
