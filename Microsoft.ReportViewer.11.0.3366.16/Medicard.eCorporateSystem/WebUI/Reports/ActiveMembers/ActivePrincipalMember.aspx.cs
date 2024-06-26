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
    public partial class ActivePrincipalMember : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            GenerateReport(sender, e);
        }

        protected void GenerateReport(object sender, EventArgs e)
        {
            ActivePrincipalMemberViewer.LocalReport.DataSources.Clear();
            ActivePrincipalMemberViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/ActiveMembers/ActivePrincipalMember.rdlc");
            ActivePrincipalMemberViewer.LocalReport.DataSources.Add(new ReportDataSource("ActivePrincipalMember", ReportHelper.ActivePrincipalMembers("02202003-000260")));

        }
    }
}