export interface User {
    Name?: string;
    Email: string;
    BirthDate?: string;
}
export interface UserLogin extends User {
    Password: string;
}