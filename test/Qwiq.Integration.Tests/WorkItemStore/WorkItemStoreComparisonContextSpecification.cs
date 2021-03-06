﻿using Qwiq.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Qwiq.WorkItemStore
{
    [DeploymentItem("Microsoft.WITDataStore32.dll")]
    [DeploymentItem("Microsoft.WITDataStore64.dll")]
    [DeploymentItem("Microsoft.TeamFoundation.WorkItemTracking.Client.dll")]
    public abstract class WorkItemStoreComparisonContextSpecification : TimedContextSpecification
    {
        protected internal IWorkItemStore Rest => RestResult.WorkItemStore;

        protected Result RestResult { get; private set; }

        protected internal IWorkItemStore Soap => SoapResult.WorkItemStore;

        protected Result SoapResult { get; private set; }

        public override void Cleanup()
        {
            RestResult?.Dispose();
            SoapResult?.Dispose();

            base.Cleanup();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public override void Given()
        {
            SoapResult = new Result { WorkItemStore = TimedAction(() => IntegrationSettings.CreateSoapStore(), "SOAP", "Create WIS") };

            RestResult = new Result { WorkItemStore = TimedAction(() => IntegrationSettings.CreateRestStore(), "REST", "Create WIS") };
        }
    }
}