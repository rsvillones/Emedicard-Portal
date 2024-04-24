<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="RequesDetails.aspx.vb" Inherits="emedicard.RequesDetails" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/AccountManager/uctl/left-menu.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctList" Src="~/AccountManager/uctl/AccountInformation.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

            <!-- left menu starts -->
			<uc:LeftNav ID="LeftNav1" runat="server" />
			<!-- left menu ends -->

            <div id="content" class="span10">
			<!-- content starts -->	
                 <uc:acctlist ID="NavAccountInfo" runat="server" />      
			<!-- content ends -->
			   
            </div>
<div id="Div2" class="span10">
            <!--- REQUEST LISTING -->
             <div class="row-fluid sortable">
				<div class="box span12" id="requestdiv" runat="server">
                    <div class="box-header well" data-original-title>
                    
						<h2><i class="icon-info-sign"></i>&nbsp;REQUEST LIST</h2>
						<div class="box-icon"> 
                            <a href="#" class="btn btn-minimize btn-round" id="lnkReply"><i class="icon-chevron-up"></i></a>&nbsp;<h3>Add Notes</h3>
                        </div>
                    </div>
                     <div class="box-content">
                            <fieldset>
                                <br />
                                <div>
                                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert alert-info" Visible="false"></asp:Label>
                                </div>
                                <br />
                                <div class="control-group">
                                    <label class="control-label" for="typeahead">Remarks</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtUploadRemarks" runat="server" Rows="4" TextMode="MultiLine" 
                                                          Width="728px" MaxLength="250"></asp:TextBox>
                                    </div>
                                </div>  
                                <div class="control-group"><label class="control-label" for="typeahead">File Name</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:FileUpload ID="FileUpload1" runat="server" /><br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Allowed files:&nbsp;&nbsp;excel file(.xls, xlsx), compressed file(.zip, .rar), pdf file, images(.jpeg, .png, .gif)<br /> &nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;15mb max file size. <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Invalid file attachment." 
                                                        CssClass="label label-warning" Display="Dynamic" ></asp:CustomValidator><br />
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="Dynamic" ControlToValidate="FileUpload1"  
                                    ClientValidationFunction = "validate_file"></asp:CustomValidator></div>  
                                <div class="control-group">
                                    <div class="controls">
                                        <div class="errmsg" style="max-width: 350px;">
                                
                                        </div>
                                    </div>
                                </div>                                                                     
                                <div class="control-group">
                                    <div class="controls">
                                        <asp:Button ID="btnUpload" runat="server" Text="Submit" CssClass="btn btn-primary" CausesValidation="False" />
                                    </div>
                                </div>
                            </fieldset>
                            <asp:Button ID="Button1" runat="server" Text="Button Cancel Hidden"  
                                style="display: none;" CausesValidation="False"/>
                     </div>
                     <div class="box-content">
                        <div>
                            <asp:Repeater ID="RepDetails" runat="server">
                            <HeaderTemplate>
                            <table style=" border:1px solid #dddddd; width:100%" cellpadding="2">
                            <tr style="background-color:#368bd7; color:White">
                            <td colspan="2" style="height: 24px;">
                            <b>Details</b>
                            </td>
                            </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                            <tr style="background-color:#EBEFF0">
                            <td>
                            <table style="background-color:#EBEFF0;border-top:1px dotted #dddddd; width:100%" >
                            <tr><td>
                            Post By: <asp:Label ID="lblUser" runat="server" Font-Bold="true" Text='<%#Eval("uploaded_by_name") %>'/> &nbsp;
                            Created Date:<asp:Label ID="lblDate" runat="server" Font-Bold="true" Text='<%#Eval("uploaded_date") %>'/>
                            </td>
                            </tr>
                            </table>
                            </td>
                            </tr>
                            <tr>
                            <td><br />
                                <asp:Label ID="lblComment" runat="server" Text='<%#Eval("remarks") %>'/><br /><br />
                            </td>
                            </tr>
                            <tr>
                            <td>
                            <table style="background-color:#EBEFF0;border-top:1px dotted #dddddd;border-bottom:1px solid #dddddd; width:100%" >
                            <tr>
                            <td>
                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("rec_id") %>' />
                                Attachment: <strong>

                                            <asp:Label ID="lblFilename" runat="server" Text='<%#Eval("file_path") %>'></asp:Label>
                                            <asp:Button ID="Button2" runat="server" Text="Download" class="btn btn-primary" CommandName="Download" CausesValidation="False"/></td>

                                            </strong>
                            </tr>
                            </table>
                            </td>
                            </tr>
                            <tr>
                            <td colspan="2">&nbsp;</td>
                            </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                            </table>
                            </FooterTemplate>
                            </asp:Repeater>


                        </div>
                     </div>
                </div>
            </div>
    </div>
</asp:Content>
