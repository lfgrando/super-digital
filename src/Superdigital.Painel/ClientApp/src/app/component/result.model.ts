export interface ResultModel<T> {
    value: T;
    statusCode: number;
    failures : any;
}