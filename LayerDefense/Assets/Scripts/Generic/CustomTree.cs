
using System;
using System.Collections.Generic;

    public class TreeNode<TKey, TValue> where TKey : IEquatable<TKey>   
    {
        private TKey key;
        private List<TValue> values;
        private List<TreeNode<TKey, TValue>> childs;

        public TreeNode(TKey key)
        {
            this.key = key;
            values = new List<TValue>();
            childs = new List<TreeNode<TKey, TValue>>();
        }

        public TreeNode(TKey key, TValue value)
        {
            this.key = key;
            values = new List<TValue>() { value };
            childs = new List<TreeNode<TKey, TValue>>();
        }

        public TKey Key
        {
            get { return key; }
            set { key = value; }
        }


        public List<TValue> Values
        {
            get { return values; }
        }


        public List<TreeNode<TKey, TValue>> Childs
        {
            get { return childs; }
        }

        public void AddChild(TreeNode<TKey, TValue> node)
        {
            childs.Add(node);
        }

        public void AddValue(TValue value)
        {
            values.Add(value);
        }

        public bool CheckKey(TKey key)
        {
            return key.Equals(Key);
        }
    }


    public class CustomKeyCategoryTree<TCategory, TValue> where TCategory : IEquatable<TCategory>
    {
        private TreeNode<TCategory, TValue> root;
        
        public CustomKeyCategoryTree(TCategory rootCategory)
        {
            root = new TreeNode<TCategory, TValue>(rootCategory);
        }

        public bool ContainsCategory(TCategory category)
        {
            var node = Find(category);
            return node != null;
        }

        public bool ContainsRootCategory(TCategory category)
        {
            var list = root.Childs;
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                if (list[i].CheckKey(category))
                    return true;
            }

            return false;
        }

        public void Add(TCategory category)
        {
            var node = new TreeNode<TCategory, TValue>(category);
            root.AddChild(node);
        }

        public void Add(TCategory category, TValue value)
        {
            var node = Find(category);
            if (node != null)
            {
                node.AddValue(value);
            }
        }

        public void Add(TCategory category, TCategory parent)
        {
            var node = Find(parent);
            if (node != null)
            {
                node.AddChild(new TreeNode<TCategory, TValue>(category));
            }
        }

        public void Add(TCategory category, TCategory parent, TValue value)
        {
            var node = Find(parent);
            if (node != null)
            {
                node.AddChild(new TreeNode<TCategory, TValue>(category, value));
            }
        }


        public TreeNode<TCategory, TValue> Find(TCategory category)
        {
            Stack<TreeNode<TCategory, TValue>> nodeStack = new Stack<TreeNode<TCategory, TValue>>();
            var rootchilds = root.Childs;
            int count = rootchilds.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                nodeStack.Push(rootchilds[i]);
            }

            TreeNode<TCategory, TValue> node = null;

            while (nodeStack.Count > 0)
            {
                node = nodeStack.Pop();
                if (node.CheckKey(category))
                {
                    nodeStack.Clear();
                    break;
                }
                else
                {
                    var childs = node.Childs;
                    int childCount = childs.Count;
                    for (int i = childCount - 1; i >= 0; i--)
                    {
                        nodeStack.Push(childs[i]);
                    }
                }
                node = null;
            }

            return node;
        }

        public TValue GetValue(Predicate<TValue> predicate)
        {
            Stack<TreeNode<TCategory, TValue>> nodeStack = new Stack<TreeNode<TCategory, TValue>>();
            var rootchilds = root.Childs;
            int count = rootchilds.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                nodeStack.Push(rootchilds[i]);
            }

            TreeNode<TCategory, TValue> node = null;
            bool isEnd = false;
            TValue value = default(TValue);
            while (nodeStack.Count > 0)
            {
                node = nodeStack.Pop();
                var list = node.Values;
                int listCount = list.Count;
                for (int i = 0; i < listCount; i++)
                {
                    if (predicate(list[i]))
                    {
                        nodeStack.Clear();
                        value = list[i];
                        isEnd = true;
                        break;
                    }
                }

                if (isEnd)
                {
                    nodeStack.Clear();
                    
                    break;
                }    
                else
                {
                    var childs = node.Childs;
                    int childCount = childs.Count;
                    for (int i = childCount - 1; i >= 0; i--)
                    {
                        nodeStack.Push(childs[i]);
                    }
                }
                node = null;
            }

            return value;
        }

        public TReturn GetFindValue<TReturn>(Func<TValue,TReturn> func)
        {
            Stack<TreeNode<TCategory, TValue>> nodeStack = new Stack<TreeNode<TCategory, TValue>>();
            var rootchilds = root.Childs;
            int count = rootchilds.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                nodeStack.Push(rootchilds[i]);
            }

            TreeNode<TCategory, TValue> node = null;
            bool isEnd = false;
            TReturn value = default(TReturn);
            while (nodeStack.Count > 0)
            {
                node = nodeStack.Pop();
                var list = node.Values;
                int listCount = list.Count;
                for (int i = 0; i < listCount; i++)
                {
                    value = func(list[i]);
                    if (value != null)
                    {
                        nodeStack.Clear();
                        isEnd = true;
                        break;
                    }
                }

                if (isEnd)
                {
                    nodeStack.Clear();

                    break;
                }
                else
                {
                    var childs = node.Childs;
                    int childCount = childs.Count;
                    for (int i = childCount - 1; i >= 0; i--)
                    {
                        nodeStack.Push(childs[i]);
                    }
                }
                node = null;
            }

            return value;
        }

        public List<TValue> GetValues(TCategory category)
        {
            var node = Find(category);
            if (node == null)
                return null;
            return node.Values;
        }

        public TValue GetValue(TCategory category, Predicate<TValue> predicate)
        {
            var node = Find(category);
            return node.Values.Find(predicate);
        }

        public TValue GetValue(TCategory category, int index = 0)
        {
            var node = Find(category);
            return node.Values[index];
        }

        public bool TryGetValues(TCategory category, out List<TValue> value)
        {
            var node = Find(category);
            if (node == null)
            {
                value = null;
                return false;
            }

            value = node.Values;
            return true;
        }

        public List<List<TValue>> GetChildValues(TCategory category)
        {
            var node = Find(category);
            if (node == null)
                return null;
            
            var list = node.Childs;
            if (list == null)
                return null;


            int count = list.Count;
            List<List<TValue>> childList = new List<List<TValue>>(count);
            for (int i = 0; i < count; i++)
            {
                childList.Add(list[i].Values);
            }

            return childList;
        }

        public List<TValue> this[TCategory category] 
        { 
            get 
            {
                return Find(category).Values;
            } 
        }
    }