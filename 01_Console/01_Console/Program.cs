using System.Runtime.InteropServices;

namespace _01_Console
{
    public enum Grade
    {
        A,
        B, 
        C,
        D, 
        F
    }

    public enum DayOfWeek
    {
        Monday,
        Tueday, 
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // 실습
            // 주사위 게임 만들기
            //RunHighLowDice();
            //RunOddEvenDice();

            //Character my = new Character();     // 메모리를 할당하여 클래스를 생성(인스턴스화) 했다.
            //my.Skill();
            //Character enemyTemp = new Character("적");     // 메모리를 할당하여 클래스를 생성(인스턴스화) 했다.
            //enemyTemp.Skill();

            Player player = new Player("나");
            //player.Skill();

            Enemy enemy = new Enemy("적");
            //enemy.Skill();

            player.Attack(enemy);

            //int i = 10;
            //Character test = new Player();      // 자식클래스의 인스턴스는 부모타입의 변수에 저장할 수 있다.
            //test.Skill();
            //Character test2 = new Player();
            //test2.Attack();

            //my.HP = 100;

            // 실습
            // 적과 나 중에 한명이 죽을 때까지 한번씩 공격하기
            // 죽을 때 누가 죽었는지 출력이 되어야 한다.
            // 한명이 죽으면 프로그램 종료
        }

        static void RunOddEvenDice()
        {
            Console.WriteLine("주사위 게임 - 홀짝");

            Console.WriteLine("게임을 시작합니다.");

            int winCount = 0;
            int select = 0;
            bool isLose = false;
            while (!isLose)
            {
                // 입력 받기 -----------------------------------------------------------------------
                
                bool isSuccess = false;       // 성공적으로 입력을 받았는지 표시하는 변수
                while (!isSuccess)  //while (isSuccess == false)
                {
                    Console.Write("홀짝 중 하나를 선택해 주세요(1-홀, 2-짝) : ");
                    string temp = Console.ReadLine();

                    int.TryParse(temp, out select);

                    if (select == 1 || select == 2)
                    {
                        isSuccess = true;       // 정상적으로 입력이 되었으면 성공이라고 표시
                    }
                }

                bool isSelectOdd;               // 선택에 따라 홀짝을 기록하기
                switch (select)                 
                {
                    case 1:
                        Console.WriteLine("당신의 선택은 홀");
                        isSelectOdd = true;
                        break;
                    case 2:
                        Console.WriteLine("당신의 선택은 짝");
                        isSelectOdd = false;
                        break;
                    default:
                        Console.WriteLine("ERROR!!!!! 있을 수 없는 선택입니다.");
                        isSelectOdd = false;
                        break;
                }

                // 주사위 굴리기 -----------------------------------------------------------------------                             

                Random r = new Random();
                int dice = r.Next(5) + 1;   // dice에는 1~6이 들어간다.                
                bool isDiceOdd = ((dice % 2) == 1) ? true : false;    // 3항 연산자를 사용했을 때.
                                                                    // (조건) ? 조건이 참일때의 값 : 조건이 거짓일 때의 값
                Console.WriteLine($"주사위 값은 {dice}가 나왔습니다.");
                if (isDiceOdd)
                {
                    Console.WriteLine("주사위 결과는 홀!");
                }
                else
                {
                    Console.WriteLine("주사위 결과는 짝!");
                }

                // 승패 확인 --------------------------------------------------------------------------------
                if (isSelectOdd == isDiceOdd)
                {
                    Console.WriteLine("당신의 승리!");
                    winCount++;
                }
                else
                {
                    Console.WriteLine("당신의 패배");
                    Console.WriteLine($"당신은 이때까지 {winCount}연승 했었습니다.");
                    isLose = true;
                }
            }

            //  2. 홀짝 게임 만들기
            //      2.1. 시작하면 홀짝 중 하나를 입력받음
            //      2.2. 주사위를 굴려서 홀이면 "홀", 짝이면 "짝"를 출력한다.
            //      2.3. 플레이어의 선택이 맞으면 성공으로 한 후 2.1로 돌아가 다시 시작한다.
            //      2.4. 플레이어의 선택이 틀리면 이때까지 몇번 성공했는지 출력하고 종료한다.
        }

        static void RunHighLowDice()
        {
            Console.WriteLine("주사위 게임 - High Low");

            Console.WriteLine("게임을 시작합니다.");

            int winCount = 0;
            int select = 0;
            bool isLose = false;
            while(!isLose)
            {
                // 입력 받기 -----------------------------------------------------------------------
                //do
                //{
                //    Console.Write("High와 Low 중 하나를 선택해 주세요(1-High, 2-Low) : ");
                //    string temp = Console.ReadLine();

                //    int.TryParse(temp, out select);
                //}
                //while (select != 1 && select != 2);   // 1 또는 2가 입력 될때까지 반복하기

                //while (true)
                //{
                //    Console.Write("High와 Low 중 하나를 선택해 주세요(1-High, 2-Low) : ");
                //    string temp = Console.ReadLine();

                //    int.TryParse(temp, out select);

                //    if(select == 1 || select == 2)
                //    {
                //        break;    // 1 또는 2가 들어오면 break를 이용해 while 끝내기
                //    }

                //    Console.WriteLine("잘못된 입력입니다. 1 또는 2를 입력해주세요.");  // 잘못된 입력이 있을 경우 경고 표시
                //}

                bool isSuccess = false;       // 성공적으로 입력을 받았는지 표시하는 변수
                while (!isSuccess)  //while (isSuccess == false)
                {
                    Console.Write("High와 Low 중 하나를 선택해 주세요(1-High, 2-Low) : ");
                    string temp = Console.ReadLine();

                    int.TryParse(temp, out select);

                    if (select == 1 || select == 2)
                    {
                        isSuccess = true;     // 정상적으로 입력이 되었으면 성공이라고 표시
                    }
                }

                bool isSelectHigh;
                switch(select)
                {
                    case 1:
                        Console.WriteLine("당신의 선택은 High");
                        isSelectHigh = true;
                        break;
                    case 2:
                        Console.WriteLine("당신의 선택은 Low");
                        isSelectHigh = false;
                        break;
                    default:
                        Console.WriteLine("ERROR!!!!! 있을 수 없는 선택입니다.");
                        isSelectHigh = false;
                        break;
                }

                // 주사위 굴리기 -----------------------------------------------------------------------
                Random r = new Random();
                int dice = r.Next(5) + 1;   // dice에는 1~6이 들어간다.
                //bool isDiceOdd = false;
                //if( dice > 3 )
                //{
                //    isDiceOdd = true;
                //}
                bool isDiceHigh = (dice > 3) ? true : false;  // 3항 연산자를 사용했을 때.
                                                          // (조건) ? 조건이 참일때의 값 : 조건이 거짓일 때의 값
                Console.WriteLine($"주사위 값은 {dice}가 나왔습니다.");
                if(isDiceHigh)
                {
                    Console.WriteLine("주사위 결과는 High!");
                }
                else
                {
                    Console.WriteLine("주사위 결과는 Low!");
                }

                // 승패 확인 --------------------------------------------------------------------------------
                if( isSelectHigh == isDiceHigh) 
                {
                    Console.WriteLine("당신의 승리!");
                    winCount++;
                }
                else
                {
                    Console.WriteLine("당신의 패배");
                    Console.WriteLine($"당신은 이때까지 {winCount}연승 했었습니다.");
                    isLose = true;
                }
            }

            //  1. High/Low만들기
            //      1.1. 시작하면 High와 Low중 하나를 입력받음
            //      1.2. 주사위를 굴려서 1~3이면 low, 4~6이면 high를 출력한다.
            //      1.3. 플레이어의 선택이 맞으면 성공으로 한 후 1.1로 돌아가 다시 시작한다.
            //      1.4. 플레이어의 선택이 틀리면 이때까지 몇번 성공했는지 출력하고 종료한다.

            //// High, high, h, H
            //string a = "High";
            //a = a.ToLower();    // 모든 영어를 소문자로 만들기
            //a = a.ToUpper();    // 모든 영어를 대문자로 만들기
        }

        private static void Day_231227()
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

            // 점수 입력 받기
            Console.Write("점수가 얼마인가요? : ");
            string inputScore = Console.ReadLine();
            float score;
            float.TryParse(inputScore, out score);
            Grade grade = GradeCheck(score);    // GradeCheck 함수의 결과를 grade에 대입

            Console.WriteLine($"당신의 등급은 [{grade}]입니다.");
            switch (grade)
            {
                case Grade.A:
                    Console.WriteLine("A등급은 ~한 해택이 있습니다.");
                    break;
                case Grade.B:
                    Console.WriteLine("Ｂ등급은 ~한 해택이 있습니다.");
                    break;
                case Grade.C:
                    Console.WriteLine("Ｃ등급은 ~한 해택이 있습니다.");
                    break;
                case Grade.D:
                    Console.WriteLine("Ｄ등급은 ~한 해택이 있습니다.");
                    break;
                case Grade.F:
                    Console.WriteLine("Ｆ등급은 해택이 없습니다.");
                    break;
                default:
                    Console.WriteLine("아무 등급도 아닙니다.");
                    break;
            }

            /// 반복문(코드를 반복하는 코드)

            //int temp2 = 0;
            //temp2 = temp2 + 1;  temp2++;    // 둘다 같은 코드

            //for문
            //for(int temp = 0; temp<10; temp++)  // (초기화;종료조건;증가량)
            //{
            //}

            Console.Write("구구단 몇 단을 출력할까요? : ");
            int.TryParse(Console.ReadLine(), out int dan);
            GuGuDan(dan);

            //while문. ()사이의 조건이 참이면 반복하는 코드
            //while()
            //{
            //}

            int count = 0;
            while (count < 10)
            {
                count++;
            }

            // do-while문. 일단 한번 실행하고 ()사이의 조건이 참이면 반복하는 코드
            count = 0;
            do
            {
                count++;
            } while (count < 10);


            // 대입 연산자
            // = :  = 왼쪽에 있는 변수에, = 오른쪽에 있는 값을 대입한다.

            // 산술 연산자
            // +
            // -
            // *
            // /
            // ++
            // --
            // += : 왼쪽에 있는 변수에 오른쪽에 있는 값을 더해서 왼쪽에 있는 변수에 대입한다.(i += 10, i에 10을 더한 후 i에 대입)
            // -=
            // *=
            // /=
            // % : 나머지 연산( i = 10 % 3; i에는 1이 들어간다.)

            // 비교 연산자
            // == : 양쪽이 같으면 true, 다르면 false
            // != : 양쪽이 다르면 true, 같으면 false
            // >
            // < 
            // >=
            // <=

            // 논리 연산자. 결과는 무조건 bool
            // && : 앤드(and), 양쪽이 모두 true일때만 true ( bool result = false && false; )
            // || : 오어(or), 양쪽 중 하나라도 true이면 true ( bool result = true || true; )
        }

        /// <summary>
        /// 구구단을 출력하는 함수
        /// </summary>
        /// <param name="dan">출력할 단 수</param>
        static void GuGuDan(int dan)
        {
            Console.WriteLine($"구구단 {dan}단 출력하기");
            for(int i=1;i<10;i++)
            {
                Console.WriteLine($"{dan} * {i} = {dan * i}");
            }
        }

        /// <summary>
        /// 점수를 받아서 A~F까지 성적을 출력하는 함수
        /// </summary>
        /// <param name="score">점수</param>
        static Grade GradeCheck(float score)
        {
            // 90점 이상 => A
            // 80점 이상 => B
            // 70점 이상 => C
            // 60점 이상 => D
            // 60점 미만 => F

            Grade grade = Grade.F;
            if(score > 89) 
            {
                Console.WriteLine("A등급입니다.");
                grade = Grade.A;
            }
            else if(score > 79)
            {
                Console.WriteLine("B등급입니다.");
                grade = Grade.B;
            }
            else if (score > 69)
            {
                Console.WriteLine("C등급입니다.");
                grade = Grade.C;
            }
            else if (score > 59)
            {
                Console.WriteLine("D등급입니다.");
                grade = Grade.D;
            }
            else
            {
                Console.WriteLine("F등급입니다.");
            }

            return grade;
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