using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbb.MyersDiffer
{
    public class MyersLand<T>
    {
        /// <summary>
        /// AsyncLock 2023-2-8 Ciaran
        /// </summary>
        private readonly SemaphoreSlim slim = new SemaphoreSlim(1, 1);

        private readonly Task<List<SnakeNode<T>>>[,,] nodes;

        public MyersLand(T[] source, T[] target)
        {
            int max_x = source.Length + 1;
            int max_y = target.Length + 1;
            this.Source = source;
            this.Target = target;
            nodes = new Task<List<SnakeNode<T>>>[max_x, max_y, 6];
        }

        public T[] Source { get; }

        public T[] Target { get; }


        public async Task<List<SnakeNode<T>>> GetSnakeBody(SnakeNode<T> node, Func<Task<List<SnakeNode<T>>>> promise)
        {
            Task<List<SnakeNode<T>>> task;
            try
            {
                await slim.WaitAsync();
                if (nodes[node.Right, node.Down, node.ToRight.GetHashCode()] != null)
                {
                    task = nodes[node.Right, node.Down, node.ToRight.GetHashCode() * 3 + node.Type - 1];
                }

                task = promise();
                nodes[node.Right, node.Down, node.ToRight.GetHashCode() * 3 + node.Type - 1] = task;
            }
            finally
            {
                slim.Release();
            }

            return await task;
        }

        public async Task<List<SnakeNode<T>>> GetSnake()
        {
            return await MyersSnake.GetSnakeBody(new SnakeNode<T>(this, true), Source, Target);
        }

        ~MyersLand()
        {
            slim.Dispose();
        }
    }
}