using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongNumbers
{
    class error : Exception
    {
        //enum for names of errors
        public enum errorCodes { divideByZero, stringMistake, emptyString, negativePower };
        //error Message
        String errorMessage;
        //public constructor
        public error(errorCodes e)
        {
            switch (e)
            {
                case errorCodes.divideByZero:
                    errorMessage = "Деление на ноль!";
                    break;
                case errorCodes.stringMistake:
                    errorMessage = "Ошибка ввода! Разрешены: 0-9 и минус в начале";
                    break;
                case errorCodes.emptyString:
                    errorMessage = "Ошибка ввода! Пустая строка";
                    break;
                case errorCodes.negativePower:
                    errorMessage = "Отрицательная степень!";
                    break;
                default:
                    errorMessage = "Unknown error";
                    break;
            }
        }
        //Property Message
        override public String Message { get { return errorMessage; } }
    }
}
