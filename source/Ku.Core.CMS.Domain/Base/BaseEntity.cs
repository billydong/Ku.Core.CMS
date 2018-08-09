﻿using Dnc.Extensions.Dapper.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ku.Core.CMS.Domain
{
    public interface IEntity
    {

    }

    public abstract class MultiKeyEntity : IEntity
    {

    }


    /// <summary>
    /// 泛型实体基类
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class Entity<TPrimaryKey> : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public TPrimaryKey Id { get; set; }
    }

    /// <summary>
    /// 默认主键类型为long的实体基类
    /// </summary>
    public abstract class BaseEntity : Entity<long>
    {
        public DateTime CreateTime { set; get; }
    }

    /// <summary>
    /// 默认主键类型为long的实体基类(虚拟删除)
    /// </summary>
    [LogicalDelete(Field = "IsDeleted")]
    public abstract class BaseProtectedEntity : BaseEntity
    {
        /// <summary>
        /// 是否被删除
        /// </summary>
        [DefaultValue(false)]
        public bool IsDeleted { set; get; } = false;
    }
}
