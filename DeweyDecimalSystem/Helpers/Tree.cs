using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeweyDecimalSystem.Models
{
    class Tree<T>
    {
        public Node<T> Root { get; set; }
        public int Count { get; set; }

        public Tree(Node<T> root)
        {
            Root = root;
            Count = 1;
        }

        public void Insert(T parent, T data)
        {
            Console.WriteLine("Insert "+data+" on "+parent);
            Node<T> parentNode = FindNodeBreadthFirst(parent);
            if(parentNode == null) throw new ArgumentException(parent + " could not be found in tree");
            parentNode.Children.Add(new Node<T>() { Data = data, Parent = parentNode });
            Count++;
        }

        public void Insert(Node<T> parent, Node<T> data)
        {
            data.Parent = parent;
            parent.Children.Add(data);
            Count++;
        }

        //https://stackoverflow.com/questions/3518508/recursively-traversing-a-tree-in-c-sharp-from-top-down-by-row
        public Node<T> FindNodeBreadthFirst(T Query)
        {
            var queue = new Queue<Node<T>>();
            queue.Enqueue(Root);
            while(queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node.Data != null && node.Data.Equals(Query))
                {
                    return node;
                }
                foreach(var child in node.Children)
                {
                    queue.Enqueue(child);
                }
            }

            return null;
        }
    }
}
