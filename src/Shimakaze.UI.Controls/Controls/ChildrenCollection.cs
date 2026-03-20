using System.Collections;

namespace Shimakaze.UI.Controls;

public sealed class ChildrenCollection(ChildrenElement element) : ICollection<UIElement?>
{
    private readonly ICollection<UIElement?> _children = [];

    public int Count => _children.Count;

    public bool IsReadOnly => _children.IsReadOnly;

    public void Add(UIElement? item)
    {
        item?.Parent = element;
        item?.VisualParentChanged += ChildrenCollection_VisualParentChanged;
        _children.Add(item);
    }

    private void ChildrenCollection_VisualParentChanged(Visual sender, EventArgs eventArgs)
    {
        if (sender.Parent == element)
            return;

        sender.VisualParentChanged -= ChildrenCollection_VisualParentChanged;
        if (sender is UIElement item)
            _children.Remove(item);
    }

    public void Clear()
    {
        foreach (var item in _children)
        {
            item?.VisualParentChanged -= ChildrenCollection_VisualParentChanged;
            item?.Parent = null;
        }

        _children.Clear();
    }

    public bool Contains(UIElement? item)
    {
        return _children.Contains(item);
    }

    public void CopyTo(UIElement?[] array, int arrayIndex)
    {
        _children.CopyTo(array, arrayIndex);
    }

    public IEnumerator<UIElement?> GetEnumerator()
    {
        return _children.GetEnumerator();
    }

    public bool Remove(UIElement? item)
    {
        item?.VisualParentChanged -= ChildrenCollection_VisualParentChanged;
        item?.Parent = null;
        return _children.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_children).GetEnumerator();
    }
}