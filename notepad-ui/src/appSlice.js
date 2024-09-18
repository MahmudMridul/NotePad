import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apis } from "./utils";

const initialState = {
   isAuth: false,

   loggedInUserName: "",
   loggedInUserEmail: "",

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
   async (obj, { dispatch, getState }) => {
      try {
         const url = apis.notesForUser;
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
         console.error("app/getNotesForUser", err);
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
            // show loading
            console.log("sign up pending");
         })
         .addCase(signUp.fulfilled, (state, action) => {
            // business logics
            console.log("sign up fulfilled");
         })
         .addCase(signUp.rejected, (state, action) => {
            // business logics
            console.log("sign up rejected");
         })

         .addCase(signIn.pending, (state, action) => {
            // show loading
         })
         .addCase(signIn.fulfilled, (state, action) => {
            if (action.payload) {
               const { data, isSuccess, message } = action.payload;
               const { name, email } = data;

               if (isSuccess) {
                  state.isAuth = true;
                  state.loggedInUserName = name;
                  state.loggedInUserEmail = email;
               }
            } else {
               console.error("Payload for login not found");
            }
         })
         .addCase(signIn.rejected, (state, action) => {
            // business logics
         })

         .addCase(getNotesForUser.pending, (state, action) => {
            //logic
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
         })
         .addCase(getNotesForUser.rejected, (state, action) => {
            //logic
         });
   },
});

export const { setState } = appSlice.actions;
export default appSlice.reducer;
