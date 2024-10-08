import { DeleteRounded, EditNoteRounded } from "@mui/icons-material";
import {
   Card,
   CardActions,
   CardContent,
   CardHeader,
   IconButton,
} from "@mui/material";
import React from "react";
import { useDispatch } from "react-redux";
import { setState } from "../appSlice";

export default function NoteItem({ note }) {
   const dispatch = useDispatch();

   function openDelModal() {
      dispatch(setState("noteId", note.id));
      dispatch(setState("delModalOpen", true));
   }

   return (
      <Card variant="outlined" sx={{ mt: 2, mb: 2 }}>
         <CardHeader title={note.title} />
         <CardContent>{note.description}</CardContent>
         <CardActions>
            <IconButton size="medium">
               <EditNoteRounded fontSize="inherit" color="primary" />
            </IconButton>

            <IconButton size="medium" onClick={openDelModal}>
               <DeleteRounded fontSize="inherit" color="error" />
            </IconButton>
         </CardActions>
      </Card>
   );
}
