using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Dijkstra
{
    //public int N { get; set; }
    //public List<Edge>[] G { get; set; }
    //public Dijkstra(int n)
    //{
    //    this.N = n;
    //    this.G = new List<Edge>[N];
    //    for (int i = 0; i < N; i++)
    //    {
    //        this.G[i] = new List<Edge>();
    //    }
    //}
    //// a ���� b �ɂȂ���ӂ�ǉ�����
    //public void Add(int a, int b, long cost = 1)
    //{
    //    this.G[a].Add(new Edge(b, cost));
    //}

    //// �P��n�_�̍ŒZ�o�H�����߂�
    //public long[] GetMinCost(int start)
    //{
    //    // �ŒZ�o�H(�R�X�g)���i�[���Ă����z��(���ׂĂ̒��_�̏����l��INF�ɂ��Ă���)
    //    var cost = new long[N];
    //    for (int i = 0; i < N; i++) cost[i] = long.MaxValue;
    //    cost[start] = 0;

    //    // ���m��̒��_���i�[����D��x�t���L���[(���_�ƃR�X�g���i�[)
    //    //var q = new PriorityQueue<P>(this.N, false);
    //    q.Enqueue(new P(start, 0));

    //    // ���m��̒��_������΂��ׂĊm�F����
    //    while (q.Count > 0)
    //    {
    //        var p = q.Dequeue();
    //        // ���łɋL�^����Ă���R�X�g�ƈقȂ�(���傫��)�ꍇ�A��������B
    //        if (p.Cost != cost[p.A]) continue;

    //        // ���o�������_���m�肷��B
    //        // �m�肵�����_���璼�ڕӂłȂ��钸�_�����[�v
    //        foreach (var e in this.G[p.A])
    //        {
    //            // ���łɋL�^����Ă���R�X�g��菬�����R�X�g�̏ꍇ
    //            if (cost[e.To] > p.Cost + e.Cost)
    //            {
    //                // �R�X�g���X�V���āA���Ƃ��ăL���[�ɓ����
    //                cost[e.To] = p.Cost + e.Cost;
    //                q.Enqueue(new P(e.To, cost[e.To]));
    //            }
    //        }
    //    }

    //    return cost;
    //}

    //// �ڑ���̒��_�ƃR�X�g���i�[����ӂ̃f�[�^
    //public struct Edge
    //{
    //    public int To;
    //    public long Cost;
    //    public Edge(int to, long cost)
    //    {
    //        this.To = to;
    //        this.Cost = cost;
    //    }
    //}
    //// ���_�Ƃ��̒��_�܂ł̃R�X�g���L�^
    //public struct P : System.IComparable<P>
    //{
    //    public int A;
    //    public long Cost;
    //    public P(int a, long cost)
    //    {
    //        this.A = a;
    //        this.Cost = cost;
    //    }
    //    public int CompareTo(P other)
    //    {
    //        return this.Cost.CompareTo(other.Cost);
    //    }
    //}
}