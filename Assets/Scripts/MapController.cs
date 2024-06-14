using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] Text fromDisplay;
    [SerializeField] Text toDisplay;
    [SerializeField] Text infoDisplay;

    Pathfinder<Node> map;
    Node to, from;
    Vector3[] linePath;

    void Start()
    {
        Node.OnClicked += HandleNodeClicked;

        var nodes = GameObject.FindObjectsOfType<Node>();
        var connections = new Dictionary<Node, HashSet<Node>>();
        foreach (var node in nodes)
        {
            connections.Add(node, new HashSet<Node>(node.Neighbors));
        }
        map = new Pathfinder<Node>(connections);
    }

    void HandleNodeClicked(Node node)
    {
        switch (from, to)
        {
            case (null, null):
                from = node;
                break;
            case ({ }, null):
                if (from == node) break;
                to = node;
                var path = map.FindPath(from, to);
                if (path == null) break;
                linePath = path.Select(n => n.Position).ToArray();
                line.positionCount = linePath.Length;
                line.SetPositions(linePath);
                break;
            case ({ }, { }):
                from = node;
                to = null;
                linePath = null;
                break;
        }
        line.enabled = from != null && to != null && linePath != null;
        toDisplay.text = to?.name ?? string.Empty;
        fromDisplay.text = from?.name ?? string.Empty;
        infoDisplay.text = linePath == null && from != null && to != null ? "path not possible" : string.Empty;
    }
}
