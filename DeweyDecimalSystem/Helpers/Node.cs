using System.Collections.Generic;

namespace DeweyDecimalSystem.Models
{
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Parent { get; set; }
        public List<Node<T>> Children { get; set; }

        public Node()
        {
            Children = new List<Node<T>>();
        }

        public Node(T data)
        {
            Data = data;
            Children = new List<Node<T>>();
        }
    }
}
