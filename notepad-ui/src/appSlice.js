import { createSlice } from "@reduxjs/toolkit";

const initialState = {};

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
   extraReducers: (builder) => {},
});

export const { setState } = appSlice.actions;
export default appSlice.reducer;
