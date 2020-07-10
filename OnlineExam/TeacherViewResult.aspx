<%@ Page Title="" Language="C#" MasterPageFile="~/TeacherHome.Master" AutoEventWireup="true" CodeBehind="TeacherViewResult.aspx.cs" Inherits="OnlineExam.TeacherViewResult" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div class="content-wrapper">
        <div class="content-header">
            <div class="container-fluid">
                <h1>Result</h1>
                <div class="table-responsive">
                <asp:GridView ID="GridView1" runat="server" class="table" ></asp:GridView>
                </div>
                </div>
            </div>
        </div>


</asp:Content>
