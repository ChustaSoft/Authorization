export interface Credentials{    
    username: string;
    email: string;
    password: string;
    phone: string;
    culture: string;
    parameters: { [id: string] : string; };
}