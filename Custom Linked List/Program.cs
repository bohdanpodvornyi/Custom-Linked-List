using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace Custom_Linked_List
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var list = new CustomLinkedList<string>();

            list.AddToFront("Item 1");
            list.AddToEnd("Item 2");
            list.Add("Item 3");

            Console.WriteLine(list.Count);
            Console.WriteLine(list.IsReadOnly);

            Console.WriteLine(list.Contains("Item 3"));
            Console.WriteLine(list.Contains("Item 4"));

            string[] array = ["apple", "pear", "orange", "cherry", "watermelon"];
            list.CopyTo(array, 1);  
            foreach (var item in array)
            {
                Console.WriteLine(item);
            }

            string[] array2 = ["apple", "pear"];
            try
            {
                list.CopyTo(array2, -1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            try
            {
                list.CopyTo(array2, 15);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            try
            {
                list.CopyTo(array2, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            foreach (var item in array2)
            {
                Console.WriteLine(item);
            }

            list.Remove("Item 2");

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }            

            Console.ReadKey();
        }
    }

    public class CustomLinkedListNode<T>()
    {
        public T Data;
        public CustomLinkedListNode<T> Next;
        public CustomLinkedListNode<T> Prev;
    }

    public interface ILinkedList<T> : ICollection<T>
    {
        void AddToFront(T item);
        void AddToEnd(T item);
    }

    public class CustomLinkedList<T>() : ILinkedList<T>
    {
        public CustomLinkedListNode<T> Start;
        public int Count
        {
            get
            { 
                int count = 0;

                if (Start != null)
                {
                    var pointer = Start;

                    while (pointer != null)
                    {
                        pointer = pointer.Next;
                        count++;
                    }
                }

                return count;
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (Start != null)
            {
                if (Start.Next is null)
                {
                    Start.Next = new CustomLinkedListNode<T>();
                    Start.Next.Data = item;
                    Start.Next.Prev = Start;
                }
                else
                {
                    var pointer = Start;
                    while (pointer.Next != null)
                    {
                        pointer = pointer.Next;
                    }
                    pointer.Next = new CustomLinkedListNode<T>();
                    pointer.Next.Data = item;
                    pointer.Next.Prev = pointer;
                }
            }
            else
            {
                Start = new CustomLinkedListNode<T>();
                Start.Data = item;
            }
        }

        public void AddToEnd(T item)
        {
            Add(item);
        }

        public void AddToFront(T item)
        {
            if (Start != null)
            {
                var pointer = Start;
                Start = new CustomLinkedListNode<T>();
                Start.Data = item;
                Start.Next = pointer;
                pointer.Prev = Start;
            } 
            else
            {
                Start = new CustomLinkedListNode<T>();
                Start.Data = item;
            }
        }

        public void Clear()
        {
            Start = null;
        }

        public bool Contains(T item)
        {
            if (Start != null)
            {
                var pointer = Start;

                while (pointer != null)
                {
                    if (pointer.Data.Equals(item))
                    {
                        return true;
                    }
                    pointer = pointer.Next;
                }
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array.Length < arrayIndex+this.Count)
            {
                throw new IndexOutOfRangeException("Cannot copy the list to the array. There's no space.");
            }
            if (arrayIndex < 0 && arrayIndex > array.Length)
            {
                throw new IndexOutOfRangeException("Cannot copy the list to the array. Invalid index.");
            }
            if (Start != null)
            {
                var pointer = Start;
                var arrayIndexEdited = arrayIndex;

                while (pointer != null)
                {
                    array[arrayIndexEdited] = pointer.Data;
                    pointer = pointer.Next;
                    arrayIndexEdited++;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (Start != null)
            {
                var pointer = Start;

                while (pointer != null)
                {
                    yield return pointer.Data;
                    pointer = pointer.Next;
                }
            }

            yield break;
        }

        public bool Remove(T item)
        {
            if (Start != null)
            {
                var pointer = Start;

                while (pointer != null)
                {
                    if (pointer.Data.Equals(item))
                    {
                        if (pointer == Start)
                        {
                            Start = pointer.Next;
                        }
                        else if (pointer.Next == null)
                        {
                            pointer.Prev.Next = null;
                        }
                        else
                        {
                            pointer.Prev.Next = pointer.Next;
                            pointer.Next.Prev = pointer.Prev;
                        }
                        return true;
                    }
                    pointer = pointer.Next;
                }
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
