import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apis } from "./utils";

const initialState = {};

export const signIn = createAsyncThunk(
   "app/signIn",
   async (obj, { dispatch, getState }) => {
      try {
      } catch (err) {
         console.error("app/signIn", err);
      }
   }
);

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
         });
   },
});

export const { setState } = appSlice.actions;
export default appSlice.reducer;
