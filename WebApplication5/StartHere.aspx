<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StartHere.aspx.cs" Inherits="MyCompanyName.StartHere" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:800px;margin:auto">
        <div class="ContentRight">
	        <div class="ContentHeader">
		        <p class="MsoNormal">
                    <b><u>Hosted payment form integration – initial post<o:p></o:p></u></b></p>
	        </div>
	        <div class="ContentBodyText">
		        <p>
			        This page is analogous to the place in shopping cart where you need to jump across to the payment
			        form.
		        </p>
	        </div>

	        <div class="FormItem">
		        <div class="FormLabel">
			        Amount:
		        </div>
		        <div class="FormInputTextOnly">
			        <asp:Label ID="lbAmount" runat="server" />
		        </div>
	        </div>				
	        <div class="FormItem">
		        <div class="FormLabel">
			        Currency:
		        </div>
		        <div class="FormInputTextOnly">
			        <asp:Label ID="lbCurrency" runat="server" />
		        </div>
	        </div>				
	        <div class="FormItem">
		        <div class="FormLabel">
			        Order ID:
		        </div>
		        <div class="FormInputTextOnly">
			        <asp:Label ID="lbOrderID" runat="server" />
		        </div>
	        </div>				
	        <div class="FormItem">
		        <div class="FormLabel">
			        Order Description:
		        </div>
		        <div class="FormInputTextOnly">
			        <asp:Label ID="lbOrderDescription" runat="server" />
		        </div>
	        </div>				
        </div>
    </div>
    </form>
    <form method="post" action="<%= m_szFormAction %>">
	    <input type="hidden" name="HashDigest" value="<%= m_szHashDigest %>" />
	    <input type="hidden" name="MerchantID" value="<%= m_szMerchantID %>" />
	    <input type="hidden" name="Amount" value="<%= m_szAmount %>" />
	    <input type="hidden" name="CurrencyCode" value="<%= m_szCurrencyCode %>" />
	    <input type="hidden" name="OrderID" value="<%= m_szOrderID %>" />
	    <input type="hidden" name="TransactionType" value="<%= m_szTransactionType %>" />
	    <input type="hidden" name="TransactionDateTime" value="<%= m_szTransactionDateTime %>" />
	    <input type="hidden" name="CallbackURL" value="<%= m_szCallbackURL %>" />
	    <input type="hidden" name="OrderDescription" value="<%= m_szOrderDescription %>" />
	    <input type="hidden" name="CustomerName" value="<%= m_szCustomerName %>" />
	    <input type="hidden" name="Address1" value="<%= m_szAddress1 %>" />
	    <input type="hidden" name="Address2" value="<%= m_szAddress2 %>" />
	    <input type="hidden" name="Address3" value="<%= m_szAddress3 %>" />
	    <input type="hidden" name="Address4" value="<%= m_szAddress4 %>" />
	    <input type="hidden" name="City" value="<%= m_szCity %>" />
	    <input type="hidden" name="State" value="<%= m_szState %>" />
	    <input type="hidden" name="PostCode" value="<%= m_szPostCode %>" />
	    <input type="hidden" name="CountryCode" value="<%= m_szCountryCode %>" />
	    <input type="hidden" name="CV2Mandatory" value="<%= m_szCV2Mandatory %>" />
	    <input type="hidden" name="Address1Mandatory" value="<%= m_szAddress1Mandatory %>" />
	    <input type="hidden" name="CityMandatory" value="<%= m_szCityMandatory %>" />
	    <input type="hidden" name="PostCodeMandatory" value="<%= m_szPostCodeMandatory %>" />
	    <input type="hidden" name="StateMandatory" value="<%= m_szStateMandatory %>" />
	    <input type="hidden" name="CountryMandatory" value="<%= m_szCountryMandatory %>" />
	    <input type="hidden" name="ResultDeliveryMethod" value="<%= m_szResultDeliveryMethod %>" />
	    <input type="hidden" name="ServerResultURL" value="<%= m_szServerResultURL %>" />
	    <input type="hidden" name="PaymentFormDisplaysResult" value="<%= m_szPaymentFormDisplaysResult %>" />
	    <input type="hidden" name="ServerResultURLCookieVariables" value="<%= m_szServerResultURLCookieVariables %>" />
	    <input type="hidden" name="ServerResultURLFormVariables" value="<%= m_szServerResultURLFormVariables %>" />
	    <input type="hidden" name="ServerResultURLQueryStringVariables" value="<%= m_szServerResultURLQueryStringVariables %>" />
        <input type="hidden" name="DisplayCancelButton" value="<%= m_szDisplayCancelButton %>" />
	    
	    <div class="FormItem">
            <div class="FormItem">
		        <div class="FormSubmit">
			        <input type="submit" value="Post Data To Payment Form" />
		        </div>
	        </div>	    
	    </div>
    </form>
</body>
</html>
