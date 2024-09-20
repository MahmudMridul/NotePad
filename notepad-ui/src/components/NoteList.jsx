import { Box } from "@mui/material";
import React from "react";
import { useSelector } from "react-redux";
import NoteItem from "./NoteItem";

export default function NoteList() {
   const states = useSelector((store) => store.app);
   const { notes } = states;
   return (
      <Box>
         {notes.map((note, index) => {
            return <NoteItem note={note} key={index} />;
         })}
      </Box>
   );
}
