//----------------------------------------------------------------
// Copyright (C) 2018 kulend 版权所有
//
// 文件名：PaymentService.cs
// 功能描述：支付方式 业务逻辑处理类
//
// 创建者：kulend@qq.com
// 创建时间：2018-02-08 13:31
//
//----------------------------------------------------------------

using AutoMapper;
using Ku.Core.CMS.Domain;
using Ku.Core.CMS.Domain.Dto.Mall;
using Ku.Core.CMS.Domain.Entity.Mall;
using Ku.Core.CMS.IService.Mall;
using Dnc.Extensions.Dapper;
using Ku.Core.Infrastructure.IdGenerator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dnc.Extensions.Dapper.Builders;

namespace Ku.Core.CMS.Service.Mall
{
    public partial class PaymentService : BaseService<Payment, PaymentDto, PaymentSearch>, IPaymentService
    {
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">条数</param>
        /// <param name="where">查询条件</param>
        /// <param name="sort">排序</param>
        /// <returns>count：条数；items：分页数据</returns>
        public override async Task<(int count, List<PaymentDto> items)> GetListAsync(int page, int size, PaymentSearch where, dynamic sort)
        {
            using (var dapper = DapperFactory.Create())
            {
                var builder = new QueryBuilder().Select<Payment>().From<Payment>().Where(where).Sort(sort as object).Limit(page, size);
                var data = await dapper.QueryPageAsync<Payment>(builder);
                return (data.count, Mapper.Map<List<PaymentDto>>(data.items, opt => {
                    opt.Items.Add("JsonDeserializeIgnore", true);
                }));
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public async Task SaveAsync(PaymentDto dto)
        {
            Payment model = Mapper.Map<Payment>(dto);
            if (model.Id == 0)
            {
                //新增
                model.Id = ID.NewID();
                model.CreateTime = DateTime.Now;
                model.IsDeleted = false;
                model.IsSnapshot = false;
                model.SnapshotCount = 0;
                model.OriginId = null;
                model.EffectiveTime = DateTime.Now;
                using (var dapper = DapperFactory.Create())
                {
                    await dapper.InsertAsync(model);
                }
            }
            else
            {
                //更新
                using (var dapper = DapperFactory.Create())
                {
                    var item = new
                    {
                        //TODO:这里进行赋值
                    };
                    await dapper.UpdateAsync<Payment>(item, new { model.Id });
                }

                //var item = await _repository.GetByIdAsync(model.Id);
                //if (item == null)
                //{
                //    throw new VinoDataNotFoundException("无法取得支付方式数据！");
                //}
                //using (var trans = await _repository.BeginTransactionAsync())
                //{
                //    //生成快照
                //    if (item.PaymentMode != model.PaymentMode
                //        || !item.PaymentConfig.Equals(model.PaymentConfig))
                //    {
                //        Payment snapshot = new Payment();
                //        snapshot.Id = ID.NewID();
                //        snapshot.IsDeleted = false;
                //        snapshot.CreateTime = DateTime.Now;
                //        snapshot.Name = item.Name;
                //        snapshot.Description = item.Description;
                //        snapshot.IsEnable = item.IsEnable;
                //        snapshot.IsMobile = item.IsMobile;
                //        snapshot.PaymentConfig = item.PaymentConfig;
                //        snapshot.PaymentMode = item.PaymentMode;
                //        snapshot.SnapshotCount = item.SnapshotCount;
                //        snapshot.EffectiveTime = item.EffectiveTime;
                //        snapshot.ExpireTime = DateTime.Now;
                //        snapshot.IsSnapshot = true;
                //        snapshot.OriginId = item.OriginId;

                //        await _repository.InsertAsync(snapshot);

                //        item.PaymentMode = model.PaymentMode;
                //        item.PaymentConfig = model.PaymentConfig;
                //        item.EffectiveTime = DateTime.Now;
                //        item.SnapshotCount = item.SnapshotCount + 1;
                //    }
                //    item.Name = model.Name;
                //    item.Description = model.Description;
                //    item.IsEnable = model.IsEnable;
                //    item.IsMobile = model.IsMobile;

                //    _repository.Update(item);
                //    await _repository.SaveAsync();

                //    trans.Commit();
                //}
            }
        }
    }
}
