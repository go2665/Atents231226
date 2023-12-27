namespace _01_Console
{
    internal class Program
    {
        static void Main(string[] args)
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

            /// 함수(function)
            /// 특정 기능을 수행하는 코드 뭉치
            /// 구성요소 : 리턴타입, 이름, 파라메터(매개변수), 함수바디(코드)
            /// 

            InputName();    // 이름을 입력받고 출력하는 함수

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