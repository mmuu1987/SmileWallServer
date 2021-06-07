using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ManagerBase 的摘要说明
/// </summary>
public abstract class ManagerBase<T> where T : class, new()
{
    public abstract List<T> GetServerList();

    /// <summary>
    /// 获得该表格的所有数据
    /// </summary>
    /// <returns></returns>
    public abstract IList<T> GetAllUser();

    /// <summary>
    /// 根据用户名字获得表格的用户名数据,如果用户名是唯一的，那么返回的集合里面只有一个元素
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public abstract IList<T> GetUserByUsername(string username);
    /// <summary>
    /// 根据站点名称获得表格的用户名数据
    /// </summary>
    /// <param name="site"></param>
    /// <returns></returns>
    public abstract IList<T> GetUseBySite(string site);

    /// <summary>
    /// 保存表格
    /// </summary>
    /// <param name="user"></param>
    public abstract void SaveUser(T user);

    /// <summary>
    /// 删除表格
    /// </summary>
    /// <param name="id"></param>
    public abstract void DeleteById(int id);

    /// <summary>
    /// 更新表格数据
    /// </summary>
    /// <param name="tu"></param>
    public abstract void UpdateUser(T tu);

}