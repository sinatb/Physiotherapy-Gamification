using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class MatrixEntry
{
    public int src;
    public int dst;
}
[CreateAssetMenu]
public class ConnectionMatrix : ScriptableObject
{
    public List<MatrixEntry> matrix;    
}
