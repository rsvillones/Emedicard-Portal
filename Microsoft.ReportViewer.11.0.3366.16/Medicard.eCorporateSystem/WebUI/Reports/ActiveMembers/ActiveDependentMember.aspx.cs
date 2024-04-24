﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corelib;
using Microsoft.Reporting.WebForms;

namespace WebUI.Reports.ActiveMembers
{
    public partial class ActiveDependentMember : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            GenerateReport(sender, e);
        }

        protected void GenerateReport(object sender, EventArgs e)
        {
            ActiveDependentMemberViewer.LocalReport.DataSources.Clear();
            ActiveDependentMemberViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/ActiveMembers/ActiveDependentMember.rdlc");
            ActiveDependentMemberViewer.LocalReport.DataSources.Add(new ReportDataSource("ActiveDependentMember", ReportHelper.ActiveDependentMembers("02202003-000260")));
        }
    }
}