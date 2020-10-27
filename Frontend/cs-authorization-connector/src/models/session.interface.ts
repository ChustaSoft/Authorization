export interface Session {
    userId: string;
    username: string;
    token: string;
    expirationDate: Date | string;
    culture: string;
}