using System.Net.Sockets;

namespace Lab02;

public class LinkedListVector
{
    public Node? Head;

    public LinkedListVector(int length)
    {
        Head = new Node();
        
        var r = new Random();
        for (int i = 0; i < length; i++)
        {
            Add(r.Next(100));
        }
    }

    public void Add(int value)
    {
        var cur = Head;
        
        while (cur.Next != null)
        {
            cur = cur.Next;
        }

        cur = new Node();
        cur.Value = value;
    }

    public void Log()
    {
        Console.Write("[");
        var cur = Head;
        while (cur.Next != null)
        {
            if (cur.Next == null)
            {
                Console.Write(cur.Value);
            }
            else
            {
                Console.Write(cur.Value + ", ");
            }

            cur = cur.Next;
        }
        Console.WriteLine("]");
    }
}


public class Node
{
    public int Value;
    public Node Next;
    
    public Node()
    {
        Value = 0;
        Next = null;
    }

    public Node(int value, Node next)
    {
        Value = value;
        Next = next;
    }
}