using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;

/// <summary>
/// TextureInfoMap 的摘要说明
/// </summary>
class TextureInfoMap : ClassMap<TextureInfo>
{
    public TextureInfoMap()
    {
        Id(x => x.Id).Column("id");//设置ID属性为主键

        Map(x => x.Uuid).Column("uuid");

        Map(x => x.Url).Column("url");

        Map(x => x.Site).Column("site");

        Table(Common.Tabel);
    }
}