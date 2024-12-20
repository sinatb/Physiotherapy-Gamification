
using System;
using System.Collections.Generic;
using System.Text;

public class CircularList<T>
{
    private readonly List<T> _elements;
    private readonly int _capacity;
    private int _index;
    
    public CircularList(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentException("Capacity cannot be negative", nameof(capacity));
        _capacity = capacity;
        _elements = new List<T>(capacity);
        _index = 0;
    }

    public void Add(T element)
    {
        if (_elements.Count < _capacity)
        {
            _elements.Add(element);
        }
        else
        {
            _elements[_index] = element;
            _index = (_index + 1) % _capacity;
        }
    }

    public int CountEquals(T element)
    {
        var result = 0;
        foreach (var e in _elements)
        {
            if (e.Equals(element))
                result++;
        }
        return result;
    }

    public override string ToString()
    {
        var result = new StringBuilder();
        foreach (var e in _elements)
        {
            result.Append(e + ", ");
        }
        return result.ToString();
    }
}
