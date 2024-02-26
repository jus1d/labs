namespace Lab03;

public class LinkedListVector : IVectorable
{
    private Node head;

    private class Node
    {
        public int Value;
        public Node Next;
        
        public Node()
        {
            Value = 0;
            Next = null;
        }

        public Node(int value)
        {
            Value = value;
            Next = null;
        }
    }

    public LinkedListVector()
    {
        var r = new Random();
        
        head = new Node(r.Next(100));
        Node cur = head;
        
        for (int i = 0; i < 5; i++)
        {
            cur.Next = new Node(r.Next(100));
            cur = cur.Next;
        }
    }
    
    public LinkedListVector(int length)
    {
        var r = new Random();
        
        head = new Node(r.Next(100));
        Node cur = head;
        
        for (int i = 0; i < length; i++)
        {
            cur.Next = new Node(r.Next(100));
            cur = cur.Next;
        }
    }

    public int this[int idx]
    {
        get
        {
            if (0 <= idx && idx <= Length)
            {
                Node cur = head;
                for (int i = 0; i < idx; i++)
                {
                    cur = cur.Next;
                }

                return cur.Value;
            }
            else
            {
                throw new IndexOutOfRangeException("Linked list index out of range");
            }
        }
        set
        {
            if (0 <= idx && idx <= Length)
            {
                Node cur = head;
                for (int i = 0; i < idx; i++)
                {
                    cur = cur.Next;
                }

                cur.Value = value;
            }
            else
            {
                throw new IndexOutOfRangeException("Linked list index out of range");
            }
        }
    }

    public int Length
    {
        get
        {
            if (head == null)
            {
                return -1;
            }

            int length = 0;
            Node cur = head;
            while (cur.Next != null)
            {
                cur = cur.Next;
                length++;
            }

            return length;
        }
    }

    public double GetNorm()
    {
        double acc = 0;
        Node cur = head;
        for (int i = 0; i < Length; i++)
        {
            acc += Math.Pow(cur.Value, 2);
            cur = cur.Next;
        }

        return Math.Sqrt(acc);
    }

    public override string ToString()
    {
        string s = $"{Length}: {{";
        var cur = head;
        while (cur.Next != null)
        {
            if (cur.Next.Next == null) s += cur.Value.ToString();
            else s += $"{cur.Value}, ";
            cur = cur.Next;
        }

        s += "}";
        return s;
    }
}