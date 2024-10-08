import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apis } from "./utils";

const initialState = {
   //FLAGS
   isAuth: false,
   isLoading: false,
   delModalOpen: false,

   loggedInUserId: -1,
   loggedInUserName: "",
   loggedInUserEmail: "",

   noteId: 0,

   notes: [],
};

export const signUp = createAsyncThunk(
   "app/signUp",
   async (obj, { dispatch, getState }) => {
      try {
         const url = apis.registration;
         const res = await fetch(url, {
            method: "POST",
            headers: {
               "Content-Type": "application/json",
               Accept: "application/json",
            },
            body: JSON.stringify(obj),
         });
         const data = await res.json();
         return data;
      } catch (err) {
         console.error("app/signUp", err);
      }
   }
);

export const signIn = createAsyncThunk(
   "app/signIn",
   async (obj, { dispatch, getState }) => {
      try {
         const url = apis.login;
         const res = await fetch(url, {
            method: "POST",
            headers: {
               "Content-Type": "application/json",
               Accept: "application/json",
            },
            credentials: "include",
            body: JSON.stringify(obj),
         });
         const data = await res.json();
         return data;
      } catch (err) {
         console.error("app/signIn", err);
      }
   }
);

export const getNotesForUser = createAsyncThunk(
   "app/getNotesForUser",
   async (email, { dispatch, getState }) => {
      try {
         const url = apis.notesForUser;
         const res = await fetch(url, {
            method: "POST",
            headers: {
               "Content-Type": "application/json",
               Accept: "application/json",
            },
            credentials: "include",
            body: JSON.stringify(email),
         });

         if (!res.ok) {
            console.error(`HTTP Status Error ${res.status} ${res.statusText}`);
         }

         const text = await res.text();
         if (!text) {
            console.error("Empty response");
         }

         try {
            const data = JSON.parse(text);
            return data;
         } catch (parseError) {
            console.error("Failed to parse JSON", text);
         }
      } catch (err) {
         console.error("app/getNotesForUser", err);
      }
   }
);

export const deleteNote = createAsyncThunk(
   "app/deleteNote",
   async (id, { dispatch, getState }) => {
      try {
         const url = `${apis.deleteNote}/${id}`;
         const res = await fetch(url, {
            method: "DELETE",
            headers: {
               "Content-Type": "application/json",
               Accept: "application/json",
            },
            credentials: "include",
         });

         if (res.status !== 410) {
            console.error(`HTTP Status Error ${res.status} ${res.statusText}`);
         }

         const text = await res.text();
         if (!text) {
            console.error("Empty response");
         }

         try {
            const data = JSON.parse(text);
            return data;
         } catch (parseError) {
            console.error(`Failed to parse text ${text}`);
         }
      } catch (err) {
         console.error("app/deleteNote", err);
      }
   }
);

export const appSlice = createSlice({
   name: "app",
   initialState,
   reducers: {
      setState: {
         prepare(name, value) {
            return {
               payload: { name, value },
            };
         },
         reducer(state, action) {
            const { name, value } = action.payload;
            state[name] = value;
         },
      },
   },
   extraReducers: (builder) => {
      builder
         .addCase(signUp.pending, (state, action) => {
            state.isLoading = true;
            console.log("sign up pending");
         })
         .addCase(signUp.fulfilled, (state, action) => {
            // business logics
            console.log("sign up fulfilled");
            state.isLoading = false;
         })
         .addCase(signUp.rejected, (state, action) => {
            // business logics
            console.log("sign up rejected");
            state.isLoading = false;
         })

         .addCase(signIn.pending, (state, action) => {
            // show loading
            state.isLoading = true;
         })
         .addCase(signIn.fulfilled, (state, action) => {
            if (action.payload) {
               const { data, isSuccess, message } = action.payload;
               const { id, name, email } = data;

               if (isSuccess) {
                  state.isAuth = true;
                  state.loggedInUserId = id;
                  state.loggedInUserName = name;
                  state.loggedInUserEmail = email;
               }
            } else {
               console.error("Payload for login not found");
            }
            state.isLoading = false;
         })
         .addCase(signIn.rejected, (state, action) => {
            // business logics
            state.isLoading = false;
         })

         .addCase(getNotesForUser.pending, (state, action) => {
            //logic
            state.isLoading = true;
         })
         .addCase(getNotesForUser.fulfilled, (state, action) => {
            if (action.payload) {
               const { data, isSuccess, message } = action.payload;
               if (isSuccess) {
                  state.notes = data;
               }
               //TODO: handle if isSuccess is false
            } else {
               console.error("getNotesForUser", action);
            }
            state.isLoading = false;
         })
         .addCase(getNotesForUser.rejected, (state, action) => {
            //logic
            state.isLoading = false;
         })

         .addCase(deleteNote.pending, (state, action) => {
            state.isLoading = true;
         })
         .addCase(deleteNote.fulfilled, (state, action) => {
            //logic
            state.isLoading = false;
         })
         .addCase(deleteNote.rejected, (state, action) => {
            state.isLoading = false;
         });
   },
});

export const { setState } = appSlice.actions;
export default appSlice.reducer;
