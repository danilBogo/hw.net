export default interface IAuthContext{
    login: (token: string) => void;
    logout: () => void;
    isAdmin: (token: string | null) => boolean;
    isAdminAuth: () => boolean;
}