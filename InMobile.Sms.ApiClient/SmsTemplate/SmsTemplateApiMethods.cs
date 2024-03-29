﻿using System;
using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A collection of SMS template-specific api endpoints.
    /// </summary>
    public interface ISmsTemplateApiMethods
    {
        /// <summary>
        /// Get all existing SMS templates on account.
        /// </summary>
        /// <returns></returns>
        List<SmsTemplate> GetAll();

        /// <summary>
        /// Get a specific SMS template by its id.
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        SmsTemplate GetById(SmsTemplateId templateId);
    }

    internal class SmsTemplateApiMethods : ISmsTemplateApiMethods
    {
        private const string V4_sms_templates = "/v4/sms/templates";

        private readonly IApiRequestHelper _requestHelper;

        public SmsTemplateApiMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public List<SmsTemplate> GetAll()
        {
            return _requestHelper.ExecuteGetAndIteratePagedResult<SmsTemplate>(resource: $"{V4_sms_templates}?pageLimit=250");
        }

        public SmsTemplate GetById(SmsTemplateId templateId)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(templateId), value: templateId);
            return _requestHelper.Execute<SmsTemplate>(method: Method.GET, resource: $"{V4_sms_templates}/{templateId}");
        }
    }
}
