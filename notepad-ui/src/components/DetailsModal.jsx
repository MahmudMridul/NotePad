import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import { useDispatch, useSelector } from "react-redux";
import { setState } from "../appSlice";

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

export default function DetailsModal() {
   const dispatch = useDispatch();
   const state = useSelector((store) => store.app);
   const { detModalOpen, modalTitle, modalDesc } = state;

   function handleClose() {
      dispatch(setState("detModalOpen", false));
   }

   return (
      <Modal open={detModalOpen} onClose={handleClose}>
         <Box sx={style}>
            <Typography variant="h6">{modalTitle}</Typography>
            <Typography variant="body1">{modalDesc}</Typography>
            <Box sx={{ mt: 3, display: "flex", justifyContent: "right" }}>
               <Button
                  color="primary"
                  variant="contained"
                  sx={{ textTransform: "none" }}
                  onClick={handleClose}
               >
                  Close
               </Button>
            </Box>
         </Box>
      </Modal>
   );
}
