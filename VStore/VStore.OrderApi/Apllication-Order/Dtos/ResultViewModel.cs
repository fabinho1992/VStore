namespace VStore.OrderApi.Apllication_Order.Dtos
{
    public class ResultViewModel
    {
        public ResultViewModel(bool isSuccess = true, string message = "")
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }

        public static ResultViewModel Success()
           => new();

        public static ResultViewModel Error(string message)
            => new(false, message);

    }

    public class ResultViewModel<T> : ResultViewModel
    {
        public ResultViewModel(T? data, bool isSuccess = true, string message = "", int? totalPage = 0)
                : base(isSuccess, message)
        {
            Data = data;
            TotalPage = totalPage;
        }

        public T? Data { get; private set; }
        public int? TotalPage { get; private set; }

        public static ResultViewModel<T> Success(T data, int? totalCount = null)
        => new(data, true, "", totalCount);

        public static ResultViewModel<T> Error(string message)
            => new(default, false, message);
    }
}
