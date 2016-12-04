using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections;
using System.Collections.ObjectModel;

namespace GSYGeo
{
    // 设置导航树数据绑定的类
    public class TreeViewBinding:ObservableCollection<string>
    {
        // 构造函数
        public TreeViewBinding()
        {
            // 初始化导航树，添加一级节点
            treeItem = new ObservableCollection<TreeViewItem>();
            treeItem.Add(new TreeViewItem { Header = "基本信息", Margin = new System.Windows.Thickness(0, 5, 0, 0) });
            treeItem.Add(new TreeViewItem { Header = "钻孔" });
            treeItem.Add(new TreeViewItem { Header = "原位测试" });
            treeItem.Add(new TreeViewItem { Header = "室内试验" });
        }

        // 属性，TreeViewItem列表
        private ObservableCollection<TreeViewItem> treeItem;
        public ObservableCollection<TreeViewItem> TreeItem
        {
            get
            {
                return treeItem;
            }
            set
            {
                treeItem = value;
            }
        }

        // 方法，重置钻孔列表
        public void ReSetZkItem(string _projectName)
        {
            treeItem[1].Items.Clear();
            ObservableCollection<TreeViewItem> items = BoreholeDataBase.ReadZkListAsTreeViewItem(_projectName);
            for (int i = 0; i < items.Count; i++)
            {
                treeItem[1].Items.Add(items[i]);
            }
        }

        // 方法，重置原位测试-静力触探列表
        public void ReSetJkItem(string _projectName)
        {
            for(int i = 0; i < treeItem[2].Items.Count; i++)
            {
                TreeViewItem parentItem = (TreeViewItem)treeItem[2].Items[i];
                if (parentItem.Header.ToString() == "静力触探")
                {
                    parentItem.Items.Clear();
                    ObservableCollection<TreeViewItem> items = CPTDataBase.ReadJkListAsTreeViewItem(_projectName);
                    for(int j = 0; j < items.Count; j++)
                    {
                        parentItem.Items.Add(items[j]);
                    }
                    parentItem.IsExpanded = true;
                }
            }
        }

        // 方法，查询二级菜单下的子项个数
        public int CountSecondTreeItem(int _index)
        {
            return treeItem[_index].Items.Count;
        }

        // 方法，查询二级菜单下是否存在某个子项
        public bool IsExistSecondTreeItem(int _index,string _s)
        {
            bool isExist = false;
            TreeViewItem parentItem = (TreeViewItem)treeItem[_index];
            for(int i = 0; i < parentItem.Items.Count; i++)
            {
                TreeViewItem item = (TreeViewItem)parentItem.Items[i];
                if (item.Header.ToString() == _s)
                {
                    isExist = true;
                }
            }
            return isExist;
        }

        // 方法，导航树二级菜单新增一个item
        public void AddItemToSecondTree(int _index,string _s)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = _s;
            treeItem[_index].Items.Add(item);
        }

        // 方法，导航树二级菜单删除一个item
        public void RemoveItemFromSecondTree(int _index, string _s)
        {
            for (int i = 0; i < treeItem[_index].Items.Count; i++)
            {
                TreeViewItem item = (TreeViewItem)treeItem[_index].Items[i];
                if (item.Header.ToString() == _s)
                {
                    treeItem[_index].Items.Remove(item);
                }
            }
        }

        // 方法，导航树三级菜单新增一个item
        public void AddItemToThirdTree(int _parentIndex,int _index,string _s)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = _s;
            TreeViewItem parentItem = (TreeViewItem)treeItem[_parentIndex].Items[_index];
            parentItem.Items.Add(item);
        }

        // 方法，导航树三级菜单删除一个item
        public void RemoveItemFromThirdTree(int _parentIndex,int _index,string _s)
        {
            TreeViewItem parentItem = (TreeViewItem)treeItem[_parentIndex].Items[_index];
            for(int i = 0; i < parentItem.Items.Count; i++)
            {
                TreeViewItem item = (TreeViewItem)parentItem.Items[i];
                if (item.Header.ToString() == _s)
                {
                    parentItem.Items.Remove(item);
                }
            }
        }
    }
}
