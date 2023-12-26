using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Quadtree
{
    private class Node
    {
        public Rect Bounds;
        public List<GroundItem> Items;
        public Node[] Children;

        public Node(Rect bounds)
        {
            Bounds = bounds;
            Items = new List<GroundItem>();
            Children = new Node[4];
        }

        public bool IsLeaf => Children[0] == null;
    }

    private Node root;
    private int maxItemsPerNode;

    public Quadtree(int maxItemsPerNode = 10, int maxX = 10000, int maxY = 10000)
    {
        Rect bounds = new Rect(0, 0, maxX, maxY);
        root = new Node(bounds);
        this.maxItemsPerNode = maxItemsPerNode;
    }

    public void Insert(Vector2 position, GroundItem item)
    {
        Insert(root, position, item);
    }

    private void Insert(Node node, Vector2 position, GroundItem item)
    {
        if (!node.Bounds.Contains(position)) return;

        if (node.IsLeaf)
        {
            node.Items.Add(item);

            if (node.Items.Count > maxItemsPerNode)
            {
                Split(node);
                ReinsertItems(node);
            }
        }
        else
        {
            foreach (var child in node.Children)
            {
                Insert(child, position, item);
            }
        }
    }

    public List<GroundItem> RetrieveInRadius(Vector2 center, float radius=0.5f)
    {
        Rect searchArea = new Rect(center.x - radius, center.y - radius, radius * 2, radius * 2);
        var allItemsInArea = Retrieve(searchArea);
        var itemsInRadius = new List<GroundItem>();

        foreach (var item in allItemsInArea)
        {
            if (Vector2.Distance(center, item.GetCoords()) <= radius)
            {
                itemsInRadius.Add(item);
            }
        }

        return itemsInRadius;
    }

    private void Split(Node node)
    {
        float halfWidth = node.Bounds.width / 2;
        float halfHeight = node.Bounds.height / 2;
        float x = node.Bounds.x;
        float y = node.Bounds.y;

        node.Children[0] = new Node(new Rect(x, y, halfWidth, halfHeight));
        node.Children[1] = new Node(new Rect(x + halfWidth, y, halfWidth, halfHeight));
        node.Children[2] = new Node(new Rect(x, y + halfHeight, halfWidth, halfHeight));
        node.Children[3] = new Node(new Rect(x + halfWidth, y + halfHeight, halfWidth, halfHeight));
    }

    private void ReinsertItems(Node node)
    {
        List<GroundItem> itemsToReinsert = new List<GroundItem>(node.Items);
        node.Items.Clear();

        foreach (GroundItem item in itemsToReinsert)
        {
            Vector2 position = item.GetCoords();

            foreach (var child in node.Children)
            {
                if (child.Bounds.Contains(position))
                {
                    child.Items.Add(item);
                    break;
                }
            }
        }
    }

    public List<GroundItem> Retrieve(Rect searchArea)
    {
        return Retrieve(root, searchArea);
    }

    private List<GroundItem> Retrieve(Node node, Rect searchArea)
    {
        var items = new List<GroundItem>();

        if (!node.Bounds.Overlaps(searchArea)) return items;

        if (node.IsLeaf)
        {
            foreach (var item in node.Items)
            {
                // Перевірте, чи знаходиться item в межах searchArea
                if (searchArea.Contains(item.GetCoords()))
                {
                    items.Add(item);
                }
            }
        }
        else
        {
            foreach (var child in node.Children)
            {
                items.AddRange(Retrieve(child, searchArea));
            }
        }

        return items;
    }

    public void Remove(Vector2 position, GroundItem item)
    {
        Remove(root, position, item);
    }

    private void Remove(Node node, Vector2 position, GroundItem item)
    {
        if (!node.Bounds.Contains(position)) return;

        if (node.IsLeaf)
        {
            node.Items.Remove(item);
        }
        else
        {
            foreach (var child in node.Children)
            {
                Remove(child, position, item);
            }
        }
    }
}
