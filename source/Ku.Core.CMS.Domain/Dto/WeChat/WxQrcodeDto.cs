//----------------------------------------------------------------
// Copyright (C) 2018 kulend ��Ȩ����
//
// �ļ�����WxQrcodeDto.cs
// ����������΢�Ŷ�ά�� ���ݴ�����
//
// �����ߣ�kulend@qq.com
// ����ʱ�䣺2018-02-04 19:13
//
//----------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using Ku.Core.CMS.Domain.Enum.WeChat;

namespace Ku.Core.CMS.Domain.Dto.WeChat
{
    /// <summary>
    /// ΢�Ŷ�ά��
    /// </summary>
    public class WxQrcodeDto : BaseProtectedDto
    {
        /// <summary>
        /// ���ں�ID
        /// </summary>
        [DataType("Hidden")]
        public long AccountId { get; set; }

        public virtual WxAccountDto Account { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        [Display(Name = "����ID")]
        public int SceneId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Display(Name = "��������")]
        public EmWxSceneType SceneType { get; set; } = EmWxSceneType.Other;

        /// <summary>
        /// ʱЧ����
        /// </summary>
        [Display(Name = "ʱЧ����")]
        public EmWxPeriodType PeriodType { get; set; } = EmWxPeriodType.Temp;

        /// <summary>
        /// ����ʱ�䣨�룩
        /// </summary>
        [Display(Name = "����ʱ��", Description = "�룬��󲻳���2592000����30�죩")]
        [Range(0, 2592000)]
        public int ExpireSeconds { get; set; } = 2592000;

        /// <summary>
        /// �����û�ID
        /// </summary>
        [Display(Name = "�û�ID")]
        public long? CreateUserId { get; set; }

        /// <summary>
        /// �����û�
        /// </summary>
        public WxUserDto CreateUser { get; set; }

        /// <summary>
        /// Ticket
        /// </summary>
        [Display(Name = "Ticket")]
        [MaxLength(256)]
        public string Ticket { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [Display(Name = "Url")]
        [MaxLength(256)]
        public string Url { get; set; }

        /// <summary>
        /// �¼�Key
        /// </summary>
        [Display(Name = "�¼�")]
        [MaxLength(64)]
        public string EventKey { get; set; }

        /// <summary>
        /// ��;
        /// </summary>
        [MaxLength(128)]
        [Display(Name = "��;")]
        public string Purpose { get; set; }

        /// <summary>
        /// ɨ�����
        /// </summary>
        [Display(Name = "ɨ�����")]
        public int ScanCount { get; set; }
    }
}
