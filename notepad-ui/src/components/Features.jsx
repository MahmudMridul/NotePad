import { Button } from "@mui/material";
import React from "react";
import { useNavigate } from "react-router-dom";

export default function Features() {
   const navigate = useNavigate();

   function gotoCreate() {
      navigate("/new");
   }
   return (
      <div>
         <Button
            color="primary"
            variant="contained"
            sx={{ textTransform: "none" }}
            onClick={gotoCreate}
         >
            Create
         </Button>
      </div>
   );
}
