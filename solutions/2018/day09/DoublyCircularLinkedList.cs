namespace AdventOfCode.Solutions._2018._9;

public class Node
{
    public Node(int data)
    {
        Data = data;
    }
        
    public long Data;
    public Node? Next;
    public Node? Prev;
}

public class DoublyCircularLinkedList
{
    private Node? _first;

    public void InsertAtBeginning(Node newNode)
    {
        InsertAtEnd(newNode);
        _first = newNode;
    }

    public void InsertAtEnd(Node newNode)
    {
        if (_first == null)
        {
            _first = newNode;
            newNode.Next = newNode;
            newNode.Prev = newNode;
        }
        else
        {
            InsertAfter(_first.Prev!, newNode);
        }
    }

    public void InsertAfter(Node refNode, Node newNode)
    {
        newNode.Prev = refNode;
        newNode.Next = refNode.Next;
        newNode.Next!.Prev = newNode;
        refNode.Next = newNode;
    }

    public void Remove(Node node)
    {
        if (_first.Next == _first)
        {
            _first = null;
        }
        else
        {
            node.Prev!.Next = node.Next;
            node.Next!.Prev = node.Prev;
            if (_first == node)
                _first = node.Next;
        }
    }
}