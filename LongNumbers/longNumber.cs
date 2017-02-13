using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongNumbers
{
    class longNumber
    {
        //long number (array of digits)
        sbyte[] digits;
        //amount of numbers
        int size;
        //true = +, false = -
        bool sign;

        //private static methods to get numbers 0, 1, 2
        private static longNumber zero()
        {
            longNumber res = new longNumber(1);
            return res;
        }
        private static longNumber one()
        {
            longNumber res = new longNumber(1);
            res.digits[0] = 1;
            return res;
        }
        private static longNumber two()
        {
            longNumber res = new longNumber(1);
            res.digits[0] = 2;
            return res;
        }

        //private function of getting module
        private longNumber module()
        {
            longNumber res = new longNumber(size);
            for (int i = 0; i < size; i++) res.digits[i] = digits[i];
            res.sign = true;
            return res;
        }

        //private constructor: k nulls
        private longNumber(int k)
        {
            size = k;
            digits = new sbyte[k];
            sign = true;
            for (int i = 0; i < k; i++) digits[i] = 0;
        }

        //public constructor: string to longNumber
        public longNumber(string s)
        {
            //Check if string is empty
            if (string.IsNullOrEmpty(s)) 
                throw new error(error.errorCodes.emptyString);
            //Check if string is right
            for (int i = 0; i < s.Length; i++)
                if (s[i] < '0' || s[i] > '9')
                    if (s[i] == '-' && i == 0) ;    //if first is minus, dont do anything, else throw error
                    else throw new error(error.errorCodes.stringMistake);
            int counter = 0;            //position of first needed symbol
            //if negative then...
            if (s[counter] == '-')
            {
                counter++;
                sign = false;
                size = s.Length - 1;
            }
            else
            {
                sign = true;
                size = s.Length;
            }
            //Missing not needed nulls
            while (counter < s.Length && s[counter] == '0')
            {
                counter++;
                size--;
            }
            //if nothing is needed then number is 0
            if (size == 0)                  
            {
                sign = true;
                digits = new sbyte[1];
                digits[0] = 0;
                size = 1;
                return;
            }
            digits = new sbyte[size];
            for (int i = 0; i < size; i++)
            {
                digits[i] = (sbyte)(s[s.Length - 1 - i] - 48);
            }
        }

        /*public copy constructor*/
        public longNumber(longNumber a)
        {
            digits = new sbyte[a.size];
            size = a.size;
            sign = a.sign;
            for (int i = 0; i < size; i++)
            {
                digits[i] = a.digits[i];
            }
        }

        /*a+b*/
        static public longNumber plus(longNumber a, longNumber b)
        {
           /* Плюс
             * Проверяем знаки. Если второе число отрицательное, то меняем знак у b и возвращаем a-b
             * Если первое отрицательное, то a+b превращается в b-|a|. Меняем знак у a и возвращаем b-a
             * Иначе либо оба положительны, либо оба отрицательны - в любом случае это сложение.
             * Создаем preRes - число из нулей, длиной на 1 больше, чем самое длинное из a,b (на случай преполнения)
             * mem - это наше "в уме" - единица при переполнении, 0 - без него.
             * Складываем меньшие остатки, пока одно из чисел не кончится
             * Потом идем и добавляем оставшиеся цифры из более длинного числа (не забывая, что могло случиться переполнение
             * (при этом сложение - это a(i)+b(i)+mem. Если это >10, то mem = 1, а цифра - [a(i)+b(i)+mem] % 10
             * Если числа закончились, а у нас все еще 1 "в уме" - припишем эту единицу.
             * И не забываем подправить preRes - длина результата
             * Далее мы просто копируем preRes в res (так как в preRes мог остаться лишний мусор)
             * Если складывали отрицательные, то и на выходе отрицательное. Иначе положительное.
             * */

            longNumber res;                 //if signs are different, that's substracting
            if (a.sign && !b.sign)          //1 + -3 = 1-3
            {                               //-3 + 1 = -(3-1) = 1-3
                b.sign = true;
                res = minus(a, b);
                b.sign = false;
                return res;
            }
            if (!a.sign && b.sign)
            {
                a.sign = true;
                res = minus(b, a);
                a.sign = false;
                return res;
            }
            //memory: 9+9=8+mem (заем десятка)
            sbyte mem = 0;
            longNumber preRes;
            if (a.size > b.size)                    //biggest size + 1 (99+9=108)
                preRes = new longNumber(a.size + 1);
            else preRes = new longNumber(b.size + 1);
            /*here we have preRes = 0000.... having enough size for summary*/
            //var for counting digits
            int counter = 0;
            while (counter < Math.Min(a.size, b.size))              //digits summs while both numbers have digits
            {
                preRes.digits[counter] = (sbyte)(a.digits[counter] + b.digits[counter] + mem);
                if (preRes.digits[counter] > 9)
                {
                    mem = 1;            //if mem is used -> digit>10 -> digit = digit%10;
                    preRes.digits[counter] = (sbyte)(preRes.digits[counter] % 10);
                }
                else mem = 0;
                counter++;
            }
            /*adding left numbers*/
            if (a.size > b.size)    //if a>b then all the left digits are in a
            {
                while (counter < a.size)
                {
                    preRes.digits[counter] = (sbyte)(a.digits[counter] + mem);
                    if (preRes.digits[counter] > 9)
                    {
                        mem = 1;
                        preRes.digits[counter] = (sbyte)(preRes.digits[counter] % 10);
                    }
                    else mem = 0;
                    counter++;
                }
            }
            else
            {
                while (counter < b.size)
                {
                    preRes.digits[counter] = (sbyte)(b.digits[counter] + mem);
                    if (preRes.digits[counter] > 9)
                    {
                        mem = 1;
                        preRes.digits[counter] = (sbyte)(preRes.digits[counter] % 10);
                    }
                    else mem = 0;
                    counter++;
                }
            }
            //if we still have mem = 1 then last digit is 1 (999+9 = (1)008
            if (mem == 1) preRes.digits[counter] = 1;
            else preRes.size--;         //else size is too big, -1

            //copying answer to res
            res = new longNumber(preRes.size);
            for (int i = 0; i < res.size; i++)
            {
                res.digits[i] = preRes.digits[i];
            }
            //if we are summing negatives, res is also negative
            if (!a.sign && !b.sign) res.sign = false;
            return res;
        }

        /*a-b*/
        static public longNumber minus(longNumber a, longNumber b)
        {
            /* Минус
             * Проверяем знаки. Если второе число отрицательное, то меняем знак у b и возвращаем a+b
             * Если первое отрицательное, то a-b превращается в -|a|-b. Меняем знак у b и возвращаем a+b (=-|a|-|b|)
             * Иначе либо оба положительны, либо оба отрицательны - в любом случае это вычитание.
             * Если a=b, то ответ 0
             * preRes будет максимальной длины a или b + 1
             * Если a>b (вычитаем из большего), то знак плюс. (2 - 1 = 1, 2 - (-1) = 3, -1 - (-2) = 1 и т.д.)
             * big - большее по модулю, small - меньшее по модулю. Теперь работает только с этими двумя числами.
             * Идем с конца (т.к. числа развернуты - с начала) и вычитаем из большего по модулю - меньшее.
             * При этом при переполнении mem ("в уме") = 1, а цифра будет остатком от деления на 10.
             * Как только меньшее число закончилось, записываем остаток от большего числа, не забывая
             * о том, что у нас все еще могло быть переполнение (как, например, из 10000 вычесть 1. После первой цифры меньшее кончится,
             * но при этом единица будет заниматься до конца)
             * В конце у нас не может остаться mem=1, так как вычитаем по модулю
             * В конце мы удаляем ненужные нули( точнее меняем preSize) и копируем preRes в res (т.к. в preRes мог остаться мусор, те же нули)
             * Не забываем про знаки, о которых говорилось в начале
             * */

            longNumber res;
            if (a.sign && !b.sign)          //if signs are different, that is summ
            {                               //3 - -2 = 3+2
                b.sign = true;              //-5 - 6 = -(5+6)
                res = plus(a, b);
                b.sign = false;
                return res;
            }
            if (!a.sign && b.sign)
            {
                b.sign = false;
                res = plus(a, b);
                b.sign = true;
                return res;
            }
            longNumber preRes;
            //bigger and smaller numbers
            longNumber big, small;
            if (a == b) return zero();      //a-a=0

            if (a > b)                  //if a>b then sign is + (5 - 3 = 2; -3 - -5 = 2)
            {
                preRes = new longNumber(Math.Max(a.size, b.size) + 1);
                preRes.sign = true;
            }
            else                        //if a<b then sign is - (3 - 5 = -2; -5 - -3 = -2)
            {
                preRes = new longNumber(Math.Max(a.size, b.size) + 1);
                preRes.sign = false;
            }
            if (a.module() > b.module())        //finding bigges module (actually, we are substracting by module)
            {
                big = a;
                small = b;
            }
            else
            {
                big = b;
                small = a;
            }
            //memory. 1 if, for instance, 5-9 happens
            sbyte mem = 0;
            //counts digits
            int counter = 0;

            while (counter < Math.Min(big.size, small.size))            //while both numbers exist...
            {                                                           //subsrtact
                preRes.digits[counter] = (sbyte)(big.digits[counter] - small.digits[counter] - mem);
                if (preRes.digits[counter] < 0)
                {
                    mem = 1;
                    preRes.digits[counter] = (sbyte)(preRes.digits[counter] + 10);
                }
                else mem = 0;
                counter++;
            }
            /*substracting left numbers*/
            while (counter < big.size)
            {                                           //other digits are biggest by module - mem
                preRes.digits[counter] = (sbyte)(big.digits[counter] - mem);
                if (preRes.digits[counter] < 0)
                {
                    mem = 1;
                    preRes.digits[counter] = (sbyte)(preRes.digits[counter] + 10);
                }
                else mem = 0;
                counter++;
                //!!!mem cant be 1 at the end (because we are substracting by module ^^)!!!
            }

            //deleting not needed nulls (changing size)
            for (int i = preRes.size - 1; i >= 0; i--)
            {
                if (preRes.digits[i] == 0) preRes.size--;
                else break;
            }

            //copying to res
            res = new longNumber(preRes.size);
            for (int i = 0; i < res.size; i++)
            {
                res.digits[i] = preRes.digits[i];
            }
            res.sign = preRes.sign;
            return res;
        }

        /*a*b*/
        public static longNumber multy(longNumber a, longNumber b)
        {
            if (a == zero() || b == zero()) return zero();      //a*0 = 0
            //array of numbers to summ
            longNumber[] resArray = new longNumber[b.size];
            for (int i = 0; i < b.size; i++)
            {                                                   //number-on-digit multypling
                resArray[i] = longOnDigit(a, b.digits[i], i);
            }
            //result of mult
            longNumber res = new longNumber(1);
            for (int i = 0; i < resArray.Length; i++)
            {                                                   //summ of all the multyplies
                res = res + resArray[i];
            }
            //getting sign
            if (a.sign == b.sign) res.sign = true;
            else res.sign = false;
            return res;
        }

        /*private function, a*b where a is longNumber and b is a digit*/
        private static longNumber longOnDigit(longNumber a, sbyte b, int pos)
        {
            if (b == 0) return zero();                                  //a*0=0
            longNumber preRes = new longNumber(a.size + 1);
            //memory. for instance, when 5*9, digit is 5, mem is 4
            sbyte mem = 0;
            for (int i = 0; i < a.size; i++)
            {
                preRes.digits[i] = (sbyte)(a.digits[i] * b + mem);
                mem = (sbyte)(preRes.digits[i] / 10);
                preRes.digits[i] = (sbyte)(preRes.digits[i] % 10);
            }
            preRes.digits[a.size] = mem;                //saving last digit
            //deleting not needed nulls
            for (int i = preRes.size - 1; i >= 0; i--)
            {
                if (preRes.digits[i] == 0) preRes.size--;
                else break;
            }
            //result
            longNumber res = new longNumber(preRes.size + pos);
            for (int i = 0; i < pos; i++)
            {                                   //adding nulls (pos is like a K from res=preRes*10^K)
                res.digits[i] = 0;
            }
            for (int i = pos; i < pos + preRes.size; i++)
            {                                   //copying other digits
                res.digits[i] = preRes.digits[i - pos];
            }
            return res;
        }
        /**
         * Итоговый алгоритм деления в столбик A/B такой (это и есть школьный алгоритм)
         * 1) Выбираем из A слева столько цифр, сколько их в B. Получаем число A1.
         * 2) Если А1 меньше чем B, то прибавляем в него еще одну цифру из А.
         * 3) Перебором всех цифр С находим самую большую, при которой "элементарное произведение" C*B
         * <= A1 (тут хорошо действовать методом дихотомии)
         * 4) Записываем цифру С в результат
         * 5) Вычитаем СЛЕВА из A "элементарное произведение" C*B
         * 6) Если A >= B Повторяем (1), иначе деление закончено и A - остаток от деления
         * */
        /*a/b*/
        public static longNumber divide(longNumber a, longNumber b)
        {
            longNumber aa = new longNumber(a);                                   //saving entered a

            if (a == zero()) return zero();     //0/b=0
            if (b == zero()) throw new error(error.errorCodes.divideByZero);    //no by zero divide!
            bool signa = a.sign;                                                //taking module, saving the signs
            bool signb = b.sign;
            a.sign = true;
            b.sign = true;
            if (a < b) return new longNumber(1);                                //if |a| < |b|, a/b = 0
            //array of numbers to summ in the end
            longNumber[] numbers = new longNumber[a.size];                      //size is the size of biggest num
            //j is amount of numbers to summ in the end
            int j = 0;
            //number of last used digit, actually, the crutch :D
            int mem;
            while (!(a < b)) //or a>=b
            {
                longNumber a1 = new longNumber(b.size);                         //taking a1 - part of number for substracting
                for (int i = a1.size - 1; i >= 0; i--)
                    a1.digits[i] = a.digits[i - a1.size + a.size];
                mem = a.size - a1.size;                                         //last used digit
                if (a1 < b)
                {
                    a1 = new longNumber(a1.ToString() + a.digits[mem - 1]);       //if a1 isnt enough for substractig,
                    mem--;                                                      //take one more digit
                }
                for (sbyte i = 1; i <= 10; i++)                                 //looking for biggest in-answer digit
                {
                    longNumber test = longOnDigit(b, i, 0);
                    if (test > a1)
                    {                                                           //if found, mult on mem and add to numbers[]
                        numbers[j] = longOnDigit(longNumber.one(), (sbyte)(i - 1), mem);
                        j++;
                        break;
                    }
                }
                a = a - (numbers[j - 1] * b);                                   //substract taken part
            }
            longNumber res = new longNumber(j);                                 //res is result
            for (int i = 0; i < j; i++)
            {                                                                   //summary
                res = res + numbers[i];
            }
            a = aa;                                                              //saving the sign and a
            b.sign = signb;
            if (a.sign == b.sign) res.sign = true;
            else res.sign = false;
            return res;
        }

        /*a%b*/
        private static longNumber mod(longNumber a, longNumber b)
        {
            longNumber res = a - a / b;
            if (res.sign == false)
            {
                res = b + res;
            }
            return res;
        }

        /*Converts longNumber to string type*/
        override public string ToString()
        {
            string res = null;
            if (!sign) res = res + '-';
            for (int i = size - 1; i >= 0; i--)
            {
                res = res + (char)(digits[i] + 48);
            }
            return res;
        }

        /*overrided >*/
        static public bool operator >(longNumber a, longNumber b)
        {
            if (!a.sign && b.sign) return false;        //-a always < +b
            if (a.sign && !b.sign) return true;         //a always > -b
            if (a.size > b.size)                        //aaa and bb
            {
                if (a.sign) return true;                //+aaa always > +bb
                else return false;                      //-aaa always < -bb
            }
            if (a.size < b.size)                        //vice versa
            {
                if (a.sign) return false;
                else return true;
            }
            for (int i = a.size - 1; i >= 0; i--)       //if the same, digit-by-digit comapring
            {
                if (a.digits[i] > b.digits[i])          //if digit from a is bigger then from b
                    if (a.sign) return true;            //if a and b > 0, a>b
                    else return false;                  //else a<b
                if (a.digits[i] < b.digits[i])
                    if (a.sign) return false;
                    else return true;
            }
            return false;
        }

        /*overrided <*/
        static public bool operator <(longNumber a, longNumber b)
        {
            if (a.sign && !b.sign) return false;        //the same as > but vice versa
            if (!a.sign && b.sign) return true;
            if (a.size < b.size)
            {
                if (a.sign) return true;
                else return false;
            }
            if (a.size > b.size)
            {
                if (a.sign) return false;
                else return true;
            }
            for (int i = a.size - 1; i >= 0; i--)
            {
                if (a.digits[i] < b.digits[i])
                    if (a.sign) return true;
                    else return false;
                if (a.digits[i] > b.digits[i])
                    if (a.sign) return false;
                    else return true;
            }
            return false;
        }

        /*overrided ==*/
        static public bool operator ==(longNumber a, longNumber b)
        {
            if (a.size != b.size) return false;                     //if sizes aren't the same, false
            if (a.sign != b.sign) return false;                     //if signs aren't the same, false
            for (int i = a.size - 1; i >= 0; i--)                   //else digit-by-digit compare
            {
                if (a.digits[i] != b.digits[i]) return false;       //if digits are not the same, false
            }
            return true;                                            //in the end, true
        }

        /*overrided !=*/
        static public bool operator !=(longNumber a, longNumber b)
        {
            if (a.size != b.size) return true;                      //if signs are the same, true
            for (int i = a.size - 1; i >= 0; i--)                   //else digit-by-digit compare
            {
                if (a.digits[i] != b.digits[i]) return true;        //if not the same, true
            }
            return false;                                           //else false
        }

        /*overrided +*/
        static public longNumber operator +(longNumber a, longNumber b)
        {
            return plus(a, b);
        }

        /*overrided -*/
        static public longNumber operator -(longNumber a, longNumber b)
        {
            return minus(a, b);
        }

        /*overrided **/
        static public longNumber operator *(longNumber a, longNumber b)
        {
            return multy(a, b);
        }

        /*overrided /*/
        static public longNumber operator /(longNumber a, longNumber b)
        {
            return divide(a, b);
        }
    }
}