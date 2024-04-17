using System.Collections.ObjectModel;

namespace RGPopup.Maui.Extensions
{
    internal static class VisualElementExtensions
    {
        [Obsolete("Use " + nameof(Element) + ".GetVisualTreeDescendants()")]
        internal static IEnumerable<Element> RgDescendants(this Element element)
        {
            var queue = new Queue<Element>(16);
            queue.Enqueue(element);

            while (queue.Count > 0)
            {
                ReadOnlyCollection<Element> children = (ReadOnlyCollection<Element>)((IElementController)queue.Dequeue()).LogicalChildren;
                for (var i = 0; i < children.Count; i++)
                {
                    Element child = children[i];
                    yield return child;
                    queue.Enqueue(child);
                }
            }
        }
    }
}
