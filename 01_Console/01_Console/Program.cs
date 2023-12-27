namespace _01_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /// 함수(function)
            /// 특정 기능을 수행하는 코드 뭉치
            /// 구성요소 : 리턴타입, 이름, 파라메터(매개변수), 함수바디(코드)
            /// 

            // InputName();    // 이름을 입력받고 출력하는 함수

            // 실습
            // 함수만들기
            // 7~43번 라인까지를 수행하는 함수 만들기
            // 함수 이름은 Day_231226

            int i;      // 인티저 타입의 변수 i를 선언
            i = 10;     // i에 10을 대입한다. (=은 대입 연산자)
            bool b = (i == 10);    // i와 10을 비교한 후 같으면 b에 true를 대입하고, 다르면 false를 대입한다. (==는 비교 연산자)
            int j;
            j = i + 10; // +-*/는 산술 연산자

            //i > j;
            //i >= j;
            //i < j;
            //i <= j;

            float f;
            f = j + 10;     // int 데이터를 float에 넣는 것은 가능
            //i = f;        // float 데이터를 int에 넣는 것은 불가능

            string str1 = "Hello ";
            string str2 = "World";
            string str3 = str1 + str2;  // str3 = "Hello World"

            string.Format("{0} {1}", str1, str2);
            string str4 = $"{str1} {str2}";

            string str5 = str1 + str2 + str1 + str2;    // 최악의 경우. 쓸대 없는 임시공간이 2개 더 발생

            bool b2 = (str1 == "Hello ");   // true
            bool b3 = (str1 == "HellO ");   // false

            // 문자열간의 비교는 피할 수 있으면 무조건 피하는 것이 이득이다.

            Console.WriteLine($"str5 = {str5}");
            Console.WriteLine($"{str1}{str2}");

            // 제어문(코드의 흐름을 제어하는 코드)

            // 20살 미만이면 미성년자입니다.
            // 20살 이상이면 성인입니다.

            int age = 10;

            // 조건문(제어문 중의 하나로 특정 조건에 따라 다른 코드를 실행하는 코드)
            //if ( age < 20 )     // age가 20보다 작으면
            //{
            //    Console.WriteLine($"{age}살은 미성년자입니다."); // 이 줄을 실행
            //}

            //if( age >= 20 )     // age가 20 이상이면
            //{
            //    Console.WriteLine($"{age}살은 성인입니다."); // 이 줄을 실행
            //}

            //if (age < 20)       // age가 20 미만인가?
            //{
            //    // age가 20 미만이면 이쪽 코드 실행
            //    Console.WriteLine($"{age}살은 미성년자입니다.");
            //}
            //else
            //{
            //    // age가 20 이상이면 이쪽 코드 실행
            //    Console.WriteLine($"{age}살은 성인입니다."); 
            //}

            Console.Write("몇살인가요? : ");
            string inputAge = Console.ReadLine();
            //age = int.Parse( inputAge );        // inputAge에 들어있는 글자를 int타입으로 변경하는 코드
            int.TryParse(inputAge, out age);
            AgeCheck(age);
            
        }

        /// <summary>
        /// 점수를 받아서 A~F까지 성적을 출력하는 함수
        /// </summary>
        /// <param name="score">점수</param>
        static void GradeCheck(float score)
        {
            // 90점 이상 => A
            // 80점 이상 => B
            // 70점 이상 => C
            // 60점 이상 => D
            // 60점 미만 => F
        }

        /// <summary>
        /// 나이를 확인해서 어떤 학교에 다니는지 출력하는 함수
        /// </summary>
        /// <param name="age">입력받은 나이</param>
        static void AgeCheck(int age)
        {
            if (age < 8)
            {
                Console.WriteLine($"{age}살은 미취학아동입니다.");
            }
            else if (age < 14)
            {
                Console.WriteLine($"{age}살은 초등학생입니다.");
            }
            else if (age < 17)
            {
                Console.WriteLine($"{age}살은 중학생입니다.");
            }
            else if (age < 20)
            {
                Console.WriteLine($"{age}살은 고등학생입니다.");
            }
        }

        private static void Day_231226()
        {
            Console.WriteLine("Hello, World!");     // 한줄을 출력하는 코드
            Console.WriteLine("고병조입니다.");

            Console.WriteLine("가가가");

            Console.Write("나나나");   // 글자를 출력하는 코드
            Console.Write("다다다");

            Console.WriteLine("라라라\n마마마");

            // 변수(Variable)
            // 변하는 숫자. 메모리에 기록해둔 값.
            // 변수를 사용하려면 미리 선언해야 한다.
            // 변수를 선언할 때는 데이터 타입과 이름을 명시해야 한다.

            // 키보드 입력을 한줄 받아서 input이라는 변수에 기록하기
            string input;                       // string 타입의 변수를 input이라는 이름으로 선언
            input = Console.ReadLine();
            // string input = Console.ReadLine();
            Console.WriteLine(input);           // input 변수의 내용을 출력하기


            // 실습
            // 시작하면 이름을 물어보고 이름을 3번 출력하는 코드 만들어보기
            Console.Write("당신의 이름은 무엇입니까? : ");
            input = Console.ReadLine();

            Console.WriteLine(input);
            Console.WriteLine(input);
            Console.WriteLine(input);

            /// 데이터 타입
            /// string : 문자열. 글자들을 저장하기 위한 데이터 타입
            /// int : 인티저. 정수형. 소수점 없는 숫자를 저장하기 위한 데이터 타입(32bit), +-21억까지는 안전.
            /// float : 플로트. 실수형. 소수점 있는 숫자를 저장하기 위한 데이터 타입(32bit), 태생적으로 오차가 있다.
            /// bool : 불. true 아니면 false만 저장하는 데이터 타입.
        }

        /// <summary>
        /// 리턴 타입 : void
        /// 이름 : InputName
        /// 파라메터 : 생략되었음, () 안에 있는 변수
        /// 함수바디 : {} 사이에 있는 코드
        /// </summary>
        static void InputName()
        {
            Console.Write("당신의 이름은 무엇입니까? : ");
            string input = Console.ReadLine();

            Console.WriteLine(input);
        }
    }
}

// 주석 코드에 포함되지 않는다.

/*
  여러줄을 주석으로 처리하는 방법
 */

/// 여러줄을 주석으로 처리하는 방법
/// ㅣㅏㅓㅣㄴ아ㅓㄹ
/// ㅣㄱ자ㅓ
/// 