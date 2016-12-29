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
    /// <summary>
    /// 设置导航树数据绑定的类
    /// </summary>
    public class TreeViewBinding:ObservableCollection<string>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TreeViewBinding()
        {
            // 初始化导航树，添加一级节点
            InitialFirstTreeItem();
        }

        /// <summary>
        /// 属性，TreeViewItem列表
        /// </summary>
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
        private ObservableCollection<TreeViewItem> treeItem;

        /// <summary>
        /// 初始化导航树，添加一级节点
        /// </summary>
        public void InitialFirstTreeItem()
        {
            treeItem = new ObservableCollection<TreeViewItem>();
            treeItem.Add(new TreeViewItem { Header = "基本信息", Margin = new System.Windows.Thickness(0, 5, 0, 0) });
            treeItem.Add(new TreeViewItem { Header = "钻孔" });
            treeItem.Add(new TreeViewItem { Header = "原位测试" });
            treeItem.Add(new TreeViewItem { Header = "室内试验" });
        }

        /// <summary>
        /// 方法，重置导航树为初始结构
        /// </summary>
        public void RefreshTreeItem()
        {
            // 清除所有节点
            treeItem.Clear();

            // 初始化导航树，添加一级节点
            InitialFirstTreeItem();
        }

        /// <summary>
        /// 方法，重置钻孔列表
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        public void ReSetZkItem(string _projectName)
        {
            treeItem[1].Items.Clear();
            ObservableCollection<TreeViewItem> items = BoreholeDataBase.ReadZkListAsTreeViewItem(_projectName);
            for (int i = 0; i < items.Count; i++)
            {
                treeItem[1].Items.Add(items[i]);
            }
        }

        /// <summary>
        /// 方法，重置原位测试-静力触探列表
        /// </summary>
        /// <param name="_projectName">项目名称</param>
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

        /// <summary>
        /// 方法，查询二级菜单下的子项个数
        /// </summary>
        /// <param name="_index">二级菜单的索引号</param>
        /// <returns></returns>
        public int CountSecondTreeItem(int _index)
        {
            return treeItem[_index].Items.Count;
        }

        /// <summary>
        /// 方法，查询二级菜单下是否存在某个子项
        /// </summary>
        /// <param name="_index">二级菜单的索引号</param>
        /// <param name="_s">要查询项的header名称</param>
        /// <returns></returns>
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

        /// <summary>
        /// 方法，导航树二级菜单新增一个item
        /// </summary>
        /// <param name="_index">二级菜单的索引号</param>
        /// <param name="_s">要增加项的header名称</param>
        public void AddItemToSecondTree(int _index,string _s)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = _s;
            treeItem[_index].Items.Add(item);
        }

        /// <summary>
        /// 方法，导航树二级菜单删除一个item
        /// </summary>
        /// <param name="_index">二级菜单的索引号</param>
        /// <param name="_s">要删除项的header名称</param>
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

        /// <summary>
        /// 方法，导航树三级菜单新增一个item
        /// </summary>
        /// <param name="_parentIndex">一级菜单的索引号</param>
        /// <param name="_index">二级菜单的索引号</param>
        /// <param name="_s">三级菜单要新增项的header名称</param>
        public void AddItemToThirdTree(int _parentIndex,int _index,string _s)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = _s;
            TreeViewItem parentItem = (TreeViewItem)treeItem[_parentIndex].Items[_index];
            parentItem.Items.Add(item);
        }

        /// <summary>
        /// 方法，导航树三级菜单删除一个item
        /// </summary>
        /// <param name="_parentIndex">一级菜单的索引号</param>
        /// <param name="_index">二级菜单的索引号</param>
        /// <param name="_s">三级菜单要删除项的header名称</param>
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
