using System;
using System.Collections.Generic;

namespace AS45
{
    class Program
    {
        static void Main(string[] args)
        {
            //1
            var x = new Surgeon();
            x.DoSomething();
            x.SayHello();
            //2

            var s = new Save();
            s.State = "A";
            var h = new History(s);
            h.AddSave();
            s.State = "B";
            h.AddSave();
            s.State = "C";
            h.AddSave();
            h.Undo();
            h.AddSave();
            h.Display();

            //3
            var f1 = new EnemyA();
            var f2 = new EnemyB();
            var med = new Mediator(f1, f2);

            f1.Attack();
            f2.Def();
        }
    }
    //template
    interface Animal
    {
        public void DoSomething();
    }

    abstract class Person : Animal
    {
        public abstract void DoSomething();
        public void SayHello()
        {
            Console.WriteLine("HELLO WORLD");
        }
    }
    class Doctor : Person
    {
        public override void DoSomething()
        {
            Console.WriteLine("I help ill men");
        }
    }

    class Surgeon : Doctor
    {
        public override void DoSomething()
        {
            Console.WriteLine("I can't do anything... i'm too busy due to the surgery");
        }
    }

    //mediator
    class Mediator
    {
        private Enemy _f1; 
        private Enemy _f2;

        public Mediator(Enemy f1, Enemy f2)
        {
            _f1 = f1;
            _f2 = f2;
            _f1.Mediator = this;
            _f2.Mediator = this;
        }

        public void response(IForce.state state)
        {
            if (state == IForce.state.AAttacks)
            {
                _f2.Def();
            }
            else if (state == IForce.state.BAttacks) {
                _f1.Def();
            }
        }
    }

    interface IForce
    {
        public void Attack();
        public void Def();
        public enum state{
            AAttacks,
            BAttacks,
            SomeoneAttacks
        }
    }
    abstract class Enemy : IForce
    {
        public Mediator Mediator { set; get; }
        public abstract void Attack();

        public abstract void Def();
    }
    class EnemyA : Enemy
    {
        
        public override void Attack()
        {
            Console.WriteLine("A Attacks B");
            Mediator.response(IForce.state.AAttacks);
        }

        public override void Def()
        {
            Console.WriteLine("A is Under Attack");
            Mediator.response(IForce.state.SomeoneAttacks);
        }
    }
    class EnemyB : Enemy
    {
        public override void Attack()
        {
            Console.WriteLine("B Attacks A");
            Mediator.response(IForce.state.BAttacks);
        }

        public override void Def()
        {
            Console.WriteLine("B is Under Attack");
            Mediator.response(IForce.state.SomeoneAttacks);
        }
    }
    //memento
    class History
    {
        List<MyData> list = new List<MyData>();
        Save Save;

        public History(Save save)
        {
            Save = save;
        }

        public void AddSave()
        {
            this.list.Add(Save.MakeSave());
        }

        public void Undo()
        {
            if (list.Count <= 0)
                throw new Exception();
            list.RemoveAt(list.Count - 1);
            Save.GetLastCopy(list[list.Count - 1]);
        }

        public void Display()
        {
            foreach (var i in list)
                Console.WriteLine(i.ToString());
        }
    }

    class Save
    {
        private string _state;
        public string State
        {
            set { _state = value; }
        }
        public MyData MakeSave()
        {
            return new MyData(_state);
        }
        public void GetLastCopy(MyData data)
        {
            _state = data.State;
        }
    }

    class MyData
    {
        private string _state;
        public string State
        {
            get { return _state; }
        }

        private DateTime _date;

        public MyData(string state)
        {
            this._state = state;
            this._date = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{_state } - {_date}";
        }
    }

}
