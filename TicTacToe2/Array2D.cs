using System.Collections;
using System.Collections.Generic;

namespace TicTacToe
{
    namespace Utils
    {
        public class Array2D<T> : IEnumerable<T>
        {
            private T[] data;
            
            public uint Width { get; }
            public uint Height { get; }

            public uint Length { get => Width * Height; }

            public Array2D(uint width, uint height)
            {
                data = new T[width * height];
                Width = width;
                Height = height;
            }

            public T this[uint x, uint y]
            {
                get { return data[y * Width + x]; }
                set { data[y * Width + x] = value; }
            }

            public T this[uint index]
            {
                get { return data[index]; }
                set { data[index] = value; }
            }

            public IEnumerator<T> GetEnumerator()
            {
                foreach(T value in data)
                {
                    yield return value;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
