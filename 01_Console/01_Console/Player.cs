using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Console
{
    internal class Player : Character   // Player 클래스가 Character 클래스를 상속 받음
    {
        public Player(string _name) : base(_name)
        {            
        }

        //public new void Attack()
        //{
        //    Console.WriteLine("플레이어가 공격한다.");
        //}

        protected override float OnSkill()    // Player의 Skill 함수는 덮어쓴다.
        {
            //base.Skill();               // Player의 부모클래스의 Skill을 실행하고
            Console.WriteLine("플레이어가 파이어볼을 사용한다."); // 자신의 코드를 실행
            return attackPower * 5.0f;
        }
    }
}
