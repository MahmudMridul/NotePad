import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import { useDispatch, useSelector } from "react-redux";
import { deleteNote, getNotesForUser, setState } from "../appSlice";

const style = {
   position: "absolute",
   top: "50%",
   left: "50%",
   transform: "translate(-50%, -50%)",
   width: 400,
   bgcolor: "background.paper",
   boxShadow: 24,
   p: 4,
   borderRadius: 3,
};

export default function DeleteItemModal() {
   const dispatch = useDispatch();
   const state = useSelector((store) => store.app);
   const { delModalOpen, noteId, loggedInUserEmail } = state;

   function handleClose() {
      dispatch(setState("delModalOpen", false));
   }

   function handleDelete() {
      dispatch(deleteNote(noteId)).then(() => {
         dispatch(getNotesForUser(loggedInUserEmail));
      });
      handleClose();
   }

   return (
      <Modal open={delModalOpen} onClose={handleClose}>
         <Box sx={style}>
            <Typography variant="h6">Delete this note?</Typography>
            <Box
               sx={{ mt: 3, display: "flex", justifyContent: "space-between" }}
            >
               <Button
                  color="primary"
                  variant="contained"
                  sx={{ textTransform: "none" }}
                  onClick={handleClose}
               >
                  Cancel
               </Button>
               <Button
                  color="error"
                  variant="contained"
                  sx={{ textTransform: "none" }}
                  onClick={handleDelete}
               >
                  Delete
               </Button>
            </Box>
         </Box>
      </Modal>
   );
}
