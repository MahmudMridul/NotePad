import React, { useState, useRef } from "react";
import { Button, Typography, Box } from "@mui/material";
import FileUploadIcon from "@mui/icons-material/FileUpload";
import AttachFileIcon from "@mui/icons-material/AttachFile";
import { useDispatch, useSelector } from "react-redux";
import { getNotesForUser, importFile } from "../appSlice";

const FileUpload = () => {
   const dispatch = useDispatch();

   const state = useSelector((store) => store.app);
   const { loggedInUserId, loggedInUserEmail } = state;

   const [selectedFile, setSelectedFile] = useState(null);
   const [uploading, setUploading] = useState(false);

   const fileInputRef = useRef(null);

   const handleFileSelect = (event) => {
      const file = event.target.files[0];
      if (file) {
         setSelectedFile(file);
      }
   };

   const handleSelectClick = () => {
      fileInputRef.current.click();
   };

   function handleUpload() {
      const formData = new FormData();
      formData.append("id", loggedInUserId);
      formData.append("file", selectedFile);
      dispatch(importFile(formData)).then(() => {
         dispatch(getNotesForUser(loggedInUserEmail));
      });
   }

   return (
      <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
         <input
            type="file"
            accept=".txt"
            onChange={handleFileSelect}
            style={{ display: "none" }}
            ref={fileInputRef}
         />

         <Button
            variant="contained"
            onClick={handleSelectClick}
            startIcon={<AttachFileIcon />}
            disabled={uploading}
            sx={{ textTransform: "none" }}
         >
            Attach File
         </Button>

         <Button
            variant="contained"
            onClick={handleUpload}
            startIcon={<FileUploadIcon />}
            disabled={!selectedFile || uploading}
            color="primary"
            sx={{ textTransform: "none" }}
         >
            Upload
         </Button>

         {selectedFile && (
            <Typography variant="body1">
               Selected file: {selectedFile.name}
            </Typography>
         )}
      </Box>
   );
};

export default FileUpload;
