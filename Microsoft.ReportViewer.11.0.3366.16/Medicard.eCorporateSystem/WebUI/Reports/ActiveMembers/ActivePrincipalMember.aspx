﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivePrincipalMember.aspx.cs" Inherits="WebUI.Reports.ActiveMembers.ActivePrincipalMember" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" %>

<!DOCTYPE html>

<html>
<head id="Head1" runat="server">
    <%--<link href="/Content/foundation/app.css" rel="stylesheet" />
    <link href="/Content/foundation/grid.css" rel="stylesheet" />
    
    <script src="/Scripts/modernizr-2.6.2.js"></script>--%>
    <style>
        html, body {
            height: 100%;
            min-height: 100%;
            position: relative;
            overflow: hidden;
        }
           .column,.columns {
            padding-left: 0 !important;
            padding-right: 0 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="height: 100%">
        <div class="row report-viewer-filter">
            <%--<div class="large-4 columns">
                <h4>As Of:</h4>
            </div>--%>
            <%--<div class="large-8 columns">
                <asp:DropDownList ID="ActiveDateDropdown" Width="150px" runat="server" AppendDataBoundItems="True" OnSelectedIndexChanged="GenerateReport" AutoPostBack="True" CssClass="large-12"></asp:DropDownList>
            </div>--%>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <rsweb:ReportViewer ID="ActivePrincipalMemberViewer" runat="server" Width="100%" Height="100%"></rsweb:ReportViewer>
            </div>
        </div>



    </form>
    <script language="javascript" type="text/javascript">
        ResizeReport();
        function ResizeReport() {
            var viewer = document.getElementById("<%= ActivePrincipalMemberViewer.ClientID %>");
            var fixedtable = document.getElementById("ActivePrincipalMemberViewer_fixedTable").getElementsByTagName('tbody')[0];;


            var window = document.documentElement.clientHeight;
            fixedtable.style.height = (window - 77) + "px";
            viewer.style.height = (window - 45) + "px";

        }

        window.onresize = function resize() { ResizeReport(); };

    </script>
</body>
</html>
