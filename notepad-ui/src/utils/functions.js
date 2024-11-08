export function isValidEmail(email) {
   const pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
   return pattern.test(email);
}

export function isEightChars(pass) {
   return pass.length >= 8;
}

export function containsBothCases(pass) {
   const pattern = /(?=.*[a-z])(?=.*[A-Z])/;
   return pattern.test(pass);
}

export function containsNumber(pass) {
   const pattern = /\d/;
   return pattern.test(pass);
}

export function containsSpecialCharacter(pass) {
   const pattern = /[!@#\$%\^\&*\)\(+=._-]/;
   return pattern.test(pass);
}
