const base = process.env.REACT_APP_API;
export const apis = {
   login: `${base}/User/login`,
   registration: `${base}/User/register`,
   notesForUser: `${base}/Note`,
   deleteNote: `${base}/Note/delete`,
   editNote: `${base}/Note/edit`,
   createNote: `${base}/Note/create`,
   file: `${base}/Note/import`,
};
