<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisplayTransactionResult.aspx.cs" Inherits="DisplayTransactionResult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:800px;margin:auto">
        <asp:Panel ID="pnMessagePanel" runat="server">
	        <div class="TransactionResultsItem">
		        <div class="TransactionResultsLabel">Payment Processor Response:</div>
		        <div class="TransactionResultsText">
			        <asp:Label ID="lbMessage" runat="server" />
		        </div>
	        </div>

            <asp:Panel ID="pnDuplicateTransactionPanel" runat="server" Visible="false">
	            <div style="color:#000;margin-top:10px">
		            A duplicate transaction means that a transaction with these details
		            has already been processed by the payment provider. The details of
		            the original transaction are given below
	            </div>
	            <div class="TransactionResultsItem" style="margin-top:10px">
		            <div class="TransactionResultsLabel">
			            Previous Transaction Response:
		            </div>
		            <div class="TransactionResultsText">
			            <asp:Label ID="lbPreviousTransactionMessage" runat="server" />
		            </div>
	            </div>
            </asp:Panel>
	        <div style="margin-top:10px">
		        <a href="StartHere.aspx">Process Another</a>
	        </div>
        </asp:Panel>    
    </div>
    </form>
</body>
</html>
