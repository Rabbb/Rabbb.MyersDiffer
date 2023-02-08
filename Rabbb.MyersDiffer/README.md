# Rabbb.MyersDiffer


There is a example at blow.


```
using System;
using Rabbb.MyersDiffer;
using Rabbb.MyersDiffer.Extension;
using static Rabbb.MyersDiffer.MyersSnake;


namespace MyersCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
            Test11();
            Test2();
            Test3();
        }


        private static void Test1()
        {
            var s1 = "ABCABBA".ToCharArray();
            var s2 = "CBABAC".ToCharArray();
            var land = new MyersLand<char>(s1, s2);

            var snake = land.GetSnake().Result;

            PrintSnake(snake);
            PrintSnake2(snake);
        }

        private static void Test11()
        {
            var s1 = "123".ToCharArray();
            var s2 = "4567".ToCharArray();
            var land = new MyersLand<char>(s1, s2);

            var snake = land.GetSnake().Result;

            PrintSnake(snake);
        }


        private static void Test2()
        {
            var s1 = new string[] { "one", "two", "three", };
            var s2 = new string[] { "four", "five", "six", "seven", };
           
            PrintSnake(s1.MyersDiff(s2), " ");
        }

        private static void Test3()
        {
            var s1 = @"
class Foo
  def initialize(name)
    @name = name
  end
end
".Split(new string[] { "\r\n" }, StringSplitOptions.None);
            var s2 = @"
class Foo
  def initialize(name)
    @name = name
  end

  def inspect
    @name
  end
end
".Split(new string[] { "\r\n" }, StringSplitOptions.None);
            var land = new MyersLand<string>(s1, s2);

            var snake = land.GetSnake().Result;

            PrintSnake(snake, "\r\n");
        }
        
    }
}
```
