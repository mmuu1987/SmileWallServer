using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// TexturePropertyManager 的摘要说明
/// </summary>
/// <summary>
/// 对数据库weidate增删改查的类
/// </summary>
public class TexturePropertyManager : ManagerBase<TextureInfo>
{

    public override List<TextureInfo> GetServerList()
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                var list = session.QueryOver<TextureInfo>();

                transaction.Commit();

                return (List<TextureInfo>)list.List<TextureInfo>();
            }
        }
    }
    /// <summary>
    /// 获得该表格的所有数据
    /// </summary>
    /// <returns></returns>
    public override IList<TextureInfo> GetAllUser()
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                var userList = session.QueryOver<TextureInfo>();
                transaction.Commit();
                return userList.List();
            }
        }
    }
    /// <summary>
    /// 根据用户名字获得表格的用户名数据,如果用户名是唯一的，那么返回的集合里面只有一个元素
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public override IList<TextureInfo> GetUserByUsername(string username)
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                var userList = session.QueryOver<TextureInfo>().Where(user => user.Uuid == username);
                transaction.Commit();
                return userList.List();
            }
        }
    }

    /// <summary>
    /// 根据站点获得表格的用户名数据
    /// </summary>
    /// <param name="username">站点</param>
    /// <returns></returns>
    public override IList<TextureInfo> GetUseBySite(string site)
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                var userList = session.QueryOver<TextureInfo>().Where(user => user.Site == site);
                transaction.Commit();
                return userList.List();
            }
        }
    }
    /// <summary>
    /// 保存表格
    /// </summary>
    /// <param name="user"></param>
    public override void SaveUser(TextureInfo user)
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }
        }
    }
    /// <summary>
    /// 删除表格
    /// </summary>
    /// <param name="id"></param>
    public override void DeleteById(int id)
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                TextureInfo tu = new TextureInfo();
                tu.Id = id;
                session.Delete(tu);
                transaction.Commit();
            }
        }
    }
    /// <summary>
    /// 更新表格数据
    /// </summary>
    /// <param name="tu"></param>
    public override void UpdateUser(TextureInfo tu)
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Update(tu);

                transaction.Commit();
            }
        }
    }
}