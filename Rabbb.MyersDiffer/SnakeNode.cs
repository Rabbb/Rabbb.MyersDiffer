using System.Collections.Generic;
using static Rabbb.MyersDiffer.MyersNodeType;

namespace Rabbb.MyersDiffer
{
    public class SnakeNode<T>
    {
        public SnakeNode(MyersLand<T> land, bool toRight)
        {
            Land = land;
            this.ToRight = toRight;
        }

        public MyersLand<T> Land { get; }

        private List<T> _body;
        public T Node { get; set; }

        public MyersNodeType Type { get; set; } = REMOVE;

        public int Down { get; set; }
        public int Right { get; set; }

        public bool ToRight { get; set; }

        public List<T> Body
        {
            get => _body = _body ?? new List<T>();
            set => _body = value;
        }

        public bool IsValid { get; set; } = true;
        
        public static SnakeNode<T> MoveRight(SnakeNode<T> node, T[] arr)
        {
            return new SnakeNode<T>(node.Land, true)
            {
                Right = node.Right + 1,
                Down = node.Down,
                IsValid = arr.Length > 0,
                Type = REMOVE,
                Node = arr.Length > 0 ? arr[0] : default,
            };
        }

        public static SnakeNode<T> MoveDown(SnakeNode<T> node, T[] arr)
        {
            return new SnakeNode<T>(node.Land, false)
            {
                Right = node.Right,
                Down = node.Down + 1,
                IsValid = arr.Length > 0,
                Type = ADD,
                Node = arr.Length > 0 ? arr[0] : default,
            };
        }

        public static SnakeNode<T> MoveSlant(SnakeNode<T> node, T value)
        {
            return new SnakeNode<T>(node.Land, true)
            {
                Right = node.Right + 1,
                Down = node.Down + 1,
                IsValid = true,
                Type = MERGE,
                Node = value,
            };
        }

        public override string ToString()
        {
            return Node is null ? "" : Node.ToString();
        }
    }
}