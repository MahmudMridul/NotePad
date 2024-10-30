import React, { useState, useRef } from "react";
import { Button, Typography, Box } from "@mui/material";
import FileUploadIcon from "@mui/icons-material/FileUpload";
import AttachFileIcon from "@mui/icons-material/AttachFile";
import { useDispatch } from "react-redux";
import { importFile } from "../appSlice";

const FileUpload = () => {
   const dispatch = useDispatch();

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
      // if (!selectedFile) {
      //    alert("Please select a file first");
      //    return;
      // }
      // try {
      //    setUploading(true);
      //    const formData = new FormData();
      //    formData.append("file", selectedFile);
      //    const response = await fetch("your-api-endpoint/upload", {
      //       method: "POST",
      //       body: formData,
      //    });
      //    if (!response.ok) {
      //       throw new Error("Upload failed");
      //    }
      //    alert("File uploaded successfully");
      //    // Clear the selected file after successful upload
      //    setSelectedFile(null);
      //    // Reset the file input
      //    fileInputRef.current.value = "";
      // } catch (error) {
      //    console.error("Error uploading file:", error);
      //    alert("Error uploading file: " + error.message);
      // } finally {
      //    setUploading(false);
      // }
      const formData = new FormData();
      formData.append("file", selectedFile);
      dispatch(importFile(formData));
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
