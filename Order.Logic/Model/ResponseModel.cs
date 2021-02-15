using System;

namespace Order.Logic.Model
{
    [Serializable]
    public class ResponseModel<T>
    {
        public bool IsSuccessfull { get; }
        public string Message { get; }
        public T Result { get; }

        protected internal ResponseModel(bool isSuccessFull, string message, T result)
        {
            IsSuccessfull = isSuccessFull;
            Message = message;
            Result = result;
        }
    }
    public sealed class ResponseModel : ResponseModel<string>
    {
        private ResponseModel(bool isSuccessFull, string message) : base(isSuccessFull, message, null)
        {

        }

        public static ResponseModel<T> Ok<T>(T result)
        {
            return new ResponseModel<T>(true, "İşlem başarılı.", result);
        }

        public static ResponseModel Ok()
        {
            return new ResponseModel(true, "İşlem başarılı.");
        }

        public static ResponseModel Error(string message)
        {
            return new ResponseModel(false, message);
        }
    }
}



