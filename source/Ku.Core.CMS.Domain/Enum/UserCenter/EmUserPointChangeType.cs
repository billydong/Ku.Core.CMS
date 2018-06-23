﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ku.Core.CMS.Domain.Enum.UserCenter
{
    /// <summary>
    /// 积分变动类型
    /// </summary>
    public enum EmUserPointChangeType : short
    {
        [Display(Name = "获得")]
        Gain = 1,

        [Display(Name = "消费")]
        Consume = 2,

        [Display(Name = "退还")]
        Refund = 3,
    }
}
