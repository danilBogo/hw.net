export default interface PaginationInfo{
    [key: string]: any
    readonly take: number;
    readonly skip: number;
}