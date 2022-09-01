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
    //// a から b につながる辺を追加する
    //public void Add(int a, int b, long cost = 1)
    //{
    //    this.G[a].Add(new Edge(b, cost));
    //}

    //// 単一始点の最短経路を求める
    //public long[] GetMinCost(int start)
    //{
    //    // 最短経路(コスト)を格納しておく配列(すべての頂点の初期値をINFにしておく)
    //    var cost = new long[N];
    //    for (int i = 0; i < N; i++) cost[i] = long.MaxValue;
    //    cost[start] = 0;

    //    // 未確定の頂点を格納する優先度付きキュー(頂点とコストを格納)
    //    //var q = new PriorityQueue<P>(this.N, false);
    //    q.Enqueue(new P(start, 0));

    //    // 未確定の頂点があればすべて確認する
    //    while (q.Count > 0)
    //    {
    //        var p = q.Dequeue();
    //        // すでに記録されているコストと異なる(より大きい)場合、無視する。
    //        if (p.Cost != cost[p.A]) continue;

    //        // 取り出した頂点を確定する。
    //        // 確定した頂点から直接辺でつながる頂点をループ
    //        foreach (var e in this.G[p.A])
    //        {
    //            // すでに記録されているコストより小さいコストの場合
    //            if (cost[e.To] > p.Cost + e.Cost)
    //            {
    //                // コストを更新して、候補としてキューに入れる
    //                cost[e.To] = p.Cost + e.Cost;
    //                q.Enqueue(new P(e.To, cost[e.To]));
    //            }
    //        }
    //    }

    //    return cost;
    //}

    //// 接続先の頂点とコストを格納する辺のデータ
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
    //// 頂点とその頂点までのコストを記録
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