import { DeleteRounded, EditNoteRounded } from "@mui/icons-material";
import DescriptionRoundedIcon from "@mui/icons-material/DescriptionRounded";
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
import { useNavigate } from "react-router-dom";

export default function NoteItem({ note }) {
   const { id, title, description } = note;
   const dispatch = useDispatch();
   const navigate = useNavigate();

   function openDelModal() {
      dispatch(setState("noteId", id));
      dispatch(setState("delModalOpen", true));
   }

   function openDetailModal() {
      setNoteDetailsInModal();
      dispatch(setState("detModalOpen", true));
   }

   function setNoteDetailsInModal() {
      dispatch(setState("modalTitle", title));
      dispatch(setState("modalDesc", description));
   }

   function gotoEditPage() {
      navigate(`/edit/${id}`, { state: { id, title, description } });
   }

   return (
      <Card variant="outlined" sx={{ mt: 2, mb: 2 }}>
         <CardHeader title={title} />
         <CardContent>{description}</CardContent>
         <CardActions>
            {/* Edit Button */}
            <IconButton size="medium" onClick={gotoEditPage}>
               <EditNoteRounded fontSize="inherit" color="primary" />
            </IconButton>

            {/* Delete Button */}
            <IconButton size="medium" onClick={openDelModal}>
               <DeleteRounded fontSize="inherit" color="error" />
            </IconButton>

            {/* Details Button */}
            <IconButton size="medium" onClick={openDetailModal}>
               <DescriptionRoundedIcon fontSize="inherit" color="primary" />
            </IconButton>
         </CardActions>
      </Card>
   );
}
