using System.Collections.Generic;
using System.Linq;

namespace Rabbb.MyersDiffer.Extension
{
    public static class MyersDifferExtension
    {
        
        public static List<SnakeNode<T>> MyersDiff<T>(this IEnumerable<T> collect1, IEnumerable<T> collect2)
        {
            var arr1 = collect1.ToArray();
            var arr2 = collect2.ToArray();

            var land = new MyersLand<T>(arr1, arr2);

            var snake = land.GetSnake().Result;

            return snake;
        }
        
        
    }
}