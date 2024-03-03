namespace Lab03;

public class LinkedListVector : IVectorable
{
    private Node head;

    private class Node
    {
        public int value = 0;
        public Node next = null;

        public Node(int value)
        {
            this.value = value;
            next = null;
        }
    }

    public LinkedListVector()
    {
        var r = new Random();
        
        head = new Node(r.Next(100));
        Node cur = head;
        
        for (int i = 0; i < 5; i++)
        {
            cur.next = new Node(r.Next(100));
            cur = cur.next;
        }
    }
    
    public LinkedListVector(int length)
    {
        var r = new Random();
        
        head = new Node(r.Next(100));
        Node cur = head;
        
        for (int i = 0; i < length; i++)
        {
            cur.next = new Node(r.Next(100));
            cur = cur.next;
        }
    }

    public int this[int idx]
    {
        get
        {
            idx--;
            if (0 <= idx && idx <= Length)
            {
                Node cur = head;
                for (int i = 0; i < idx; i++)
                {
                    cur = cur.next;
                }

                return cur.value;
            }
            else
            {
                throw new IndexOutOfRangeException("Индекс за границами связного списка");
            }
        }
        set
        {
            idx--;
            if (0 <= idx && idx <= Length)
            {
                Node cur = head;
                for (int i = 0; i < idx; i++)
                {
                    cur = cur.next;
                }

                cur.value = value;
            }
            else
            {
                throw new IndexOutOfRangeException("Индекс за границами связного списка");
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
            while (cur.next != null)
            {
                cur = cur.next;
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
            acc += Math.Pow(cur.value, 2);
            cur = cur.next;
        }

        return Math.Sqrt(acc);
    }
    
    public override string ToString()
    {
        string s = $"{Length}: {{";

        var cur = head;

        while (cur.next != null)
        {
            if (cur.next.next == null) s += cur.value.ToString();
            else s += $"{cur.value}, ";
            cur = cur.next;
        }

        s += "}";
        return s;
    }

    public void AddToStart(int value)
    {
        Node tmp = new Node(value);
        tmp.next = head;
        head = tmp;
    }
    
    public void AddToEnd(int value)
    {
        AddByIndex(Length, value);
    }

    public void AddByIndex(int idx, int value)
    {
        idx--; // Index for users should start with 1
        
        if (0 <= idx && idx <= Length)
        {
            Node cur = head;
            for (int i = 0; i < idx - 1; i++)
            {
                cur = cur.next;
            }

            Node tmp = new Node(value);
            tmp.next = cur.next;
            cur.next = tmp;
        }
        else
        {
            throw new IndexOutOfRangeException("Индекс за границами связного списка");
        }
    }

    public void DeleteFromStart()
    {
        if (Length == 0) throw new Exception("Связный список пуст");
        
        head = head.next;
    }

    public void DeleteFromEnd()
    {
        if (Length == 0) throw new Exception("Связный список пуст");
        
        Node cur = head;
        for (int i = 0; i < Length - 1; i++)
        {
            cur = cur.next;
        }

        cur.next = null;
    }
    
    public void DeleteByIndex(int idx)
    {
        idx--; // Index for users should start with 1
        
        if (head == null) throw new Exception("Связный список пуст");
        if (idx < 0 || idx >= Length) throw new IndexOutOfRangeException("Индекс за границами связного списка");

        Node cur = head;

        if (idx == 0) 
        {
            head = cur.next;
            return;
        }

        for (int i = 0; cur != null && i < idx - 1; i++)
        {
            cur = cur.next;
        }

        if (cur == null || cur.next == null) return;

        cur.next = cur.next.next;
    }

    public void Log(string message = "")
    {
        if (message != "") Console.Write($"{message}: ");
        
        var cur = head;
        Console.Write("{");
        while (cur.next != null)
        {
            if (cur.next.next == null)
            {
                Console.Write(cur.value);
            }
            else
            {
                Console.Write(cur.value + ", ");
            }
            cur = cur.next;
        }
        Console.WriteLine("}");
    }
}