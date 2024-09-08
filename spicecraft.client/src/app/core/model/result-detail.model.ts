export interface ResultDetailModel<T> {
  message: string;                 // Represents Message
  data?: T | null;                 // Nullable T
  dataList?: T[] | null;           // Nullable array of T
  isSuccess: boolean;              // Represents IsSuccess
  notify: boolean;                 // Represents Notify
  statusCode: number;              // Assuming HttpStatusCode is a numeric enum
}
