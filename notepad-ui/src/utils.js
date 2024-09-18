const base = process.env.REACT_APP_API;
export const apis = {
   login: `${base}/User/login`,
   registration: `${base}/User/register`,
   notesForUser: `${base}/Note`,
};
