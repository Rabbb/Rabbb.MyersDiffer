using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static Rabbb.MyersDiffer.MyersNodeType;

namespace Rabbb.MyersDiffer
{
    public static class MyersSnake
    {
        private static async Task<List<SnakeNode<T>>> GetRightBody<T>(SnakeNode<T> node1, T[] s1, T[] s2)
        {
            var body1 = new List<SnakeNode<T>>();
            // move right
            body1.Add(node1);
            if (s1.Length > 0 || s2.Length > 0)
            {
                body1.AddRange(await GetSnakeBody<T>(node1, s1, s2));
            }

            return body1;
        }


        private static async Task<List<SnakeNode<T>>> GetDownBody<T>(SnakeNode<T> node2, T[] s1, T[] s2)
        {
            var body2 = new List<SnakeNode<T>>();
            // move down
            body2.Add(node2);
            if (s1.Length > 0 || s2.Length > 0)
            {
                body2.AddRange(await GetSnakeBody<T>(node2, s1, s2));
            }

            return body2;
        }

        private static List<SnakeNode<T>> GetStraightDownBody<T>(SnakeNode<T> head, T[] s2)
        {
            return s2.Select((c, i) => new SnakeNode<T>(head.Land, true) // move straight down  
            {
                Node = c,
                Right = head.Right,
                Down = head.Down + i + 1,
                Type = ADD,
            }).ToList();
        }

        private static List<SnakeNode<T>> GetStraightRightBody<T>(SnakeNode<T> head, T[] s1)
        {
            return s1.Select((c, i) => new SnakeNode<T>(head.Land, false) // move straight right  
            {
                Node = c,
                Right = head.Right + i + 1,
                Down = head.Down,
                Type = REMOVE,
            }).ToList();
        }

        private static async Task<List<SnakeNode<T>>> GetSlantBody<T>(SnakeNode<T> head, T[] s1, T[] s2)
        {
            var body1 = new List<SnakeNode<T>>();
            var end = head;
            body1.Add(head);

            int i = 0;
            for (; i < s1.Length && i < s2.Length && Equals(s1[i], s2[i]); i++)
            {
                head.Right++;
                head.Down++;
                head.Body.Add(s1[i]);
            }

            if (i > 0)
            {
                end = new SnakeNode<T>(head.Land, head.ToRight)
                {
                    Right = head.Right,
                    Down = head.Down,
                    Type = MERGE,
                    Node = head.Body[head.Body.Count - 1],
                };
                s1 = s1.Skip(i).ToArray();
                s2 = s2.Skip(i).ToArray();
            }

            if (s1.Length > 0 || s2.Length > 0)
            {
                body1.AddRange(await GetSnakeBody(end, s1, s2));
            }

            return body1;
        }


        public static async Task<List<SnakeNode<T>>> GetSnakeBody<T>(SnakeNode<T> head, T[] s1, T[] s2)
        {
            return await head.Land.GetSnakeBody(head, () => GetShakeBodyTask(head, s1, s2));
        }

        private static async Task<List<SnakeNode<T>>> GetShakeBodyTask<T>(SnakeNode<T> head, T[] s1, T[] s2)
        {
            var node1 = SnakeNode<T>.MoveRight(head, s1);
            var node2 = SnakeNode<T>.MoveDown(head, s2);


            if (!node1.IsValid)
                return !node2.IsValid
                    ? new List<SnakeNode<T>>()
                    : GetStraightDownBody(head, s2);

            if (!node2.IsValid)
                return GetStraightRightBody(head, s1);

            // 2023-2-8 Ciaran move slant 
            if (Equals(node2.Node, node1.Node))
            {
                var node3 = SnakeNode<T>.MoveSlant(head, node2.Node);

                return await GetSlantBody(node3, s1.Skip(1).ToArray(), s2.Skip(1).ToArray());
            }

            var body1 = await GetRightBody(node1, s1.Skip(1).ToArray(), s2);
            var body2 = await GetDownBody(node2, s1, s2.Skip(1).ToArray());

            // 2023-2-8 Ciaran select smooth snake body 
            if (body1.Count == body2.Count)
            {
                // 2023-2-8 Ciaran remove , add , slant style
                bool b_left = head.ToRight && body1.FirstOrDefault()?.Type == ADD && body1.SkipWhile(p=> p.Type == ADD).FirstOrDefault()?.Type == MERGE;
                bool b_right = head.ToRight && body2.FirstOrDefault()?.Type == ADD && body2.SkipWhile(p=> p.Type == ADD).FirstOrDefault()?.Type == MERGE;

                if (b_left) return body1;
                if (b_right) return body2;
                
                // 2023-2-8 Ciaran continue style
                return head.ToRight ? body1 : body2;
            }

            // 2023-2-8 Ciaran select shortest snake body
            return body1.Count <= body2.Count ? body1 : body2;
        }

        public static void PrintSnake<T>(List<SnakeNode<T>> snake, string split = null)
        {
            var backColor = Console.BackgroundColor;
            foreach (var node in snake)
            {
                switch (node.Type)
                {
                    // 2023-2-8 Ciaran right
                    case REMOVE:
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write(node.Node);
                        Console.BackgroundColor = backColor;
                        Console.Write(split);
                        break;
                    // 2023-2-8 Ciaran down
                    case ADD:
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write(node.Node);
                        Console.BackgroundColor = backColor;
                        Console.Write(split);
                        break;
                    // 2023-2-8 Ciaran slant
                    case MERGE:
                        Console.BackgroundColor = backColor;
                        Console.Write(node.Node);
                        Console.Write(split);
                        if (node.Right > 1)
                        {
                            foreach (var node1 in node.Body)
                            {
                                Console.Write(node1);
                                Console.Write(split);
                            }
                        }

                        break;
                    default:
                        throw new ArgumentException("Snake has invalid type node.");
                }
            }

            Console.BackgroundColor = backColor;
        }
    }
}