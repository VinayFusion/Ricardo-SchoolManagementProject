using SchoolManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Common
{
    public class PrintClass
    {
        string First_Word_AppName = ConfigurationManager.AppSettings["First_Word_AppName"];
        string Second_Word_AppName = ConfigurationManager.AppSettings["Second_Word_AppName"];
        string AppLogo = ConfigurationManager.AppSettings["SiteURL"] + ConfigurationManager.AppSettings["AppLogoPath"];

        public string Get_InvoicePrintableData(PayFeeListData_VM _data)
        {
            if (_data.BranchName == "")
            {
                _data.BranchName = First_Word_AppName + " " + Second_Word_AppName;
            }
            if (_data.BranchAddress == "")
            {
                _data.BranchAddress = "Mohali";
            }
            if (_data.BranchMobileNumber == "")
            {
                _data.BranchMobileNumber = "+91234567890";
            }
            string print_data = @"<!doctype html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Invoice</title>

    <style>
        .invoice-box{
            max-width:800px;
            margin:auto;
            padding:10px 30px;
            border:1px solid #eee;
            box-shadow:0 0 10px rgba(0, 0, 0, .15);
            font-size:14px;
            line-height:24px;
            font-family:'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
            color:#555;
        }

        .invoice-box table{
            width:100%;
            line-height:30px;
            text-align:left;
        }

        .invoice-box table td{
            padding:5px;
            vertical-align:top;
        }

        .invoice-box table tr td:nth-child(2){
            /*text-align:right;*/
        }

        .invoice-box table tr.top table td{
            padding-bottom:20px;
        }

        .invoice-box table tr.top table td.title{
            font-size:45px;
            line-height:45px;
            color:#333;
        }

        .invoice-box table tr.information table td{
            padding-bottom:40px;
        }

        .invoice-box table tr.heading td{
            background:#eee;
            border-bottom:1px solid #ddd;
            font-weight:bold;
        }

        .invoice-box table tr.details td{
            padding-bottom:20px;
        }

        .invoice-box table tr.item td{
            border-bottom:1px solid #eee;
        }

        .invoice-box table tr.item.last td{
            border-bottom:none;
        }

        .invoice-box table tr.total td:nth-child(2){
            border-top:2px solid #eee;
            font-weight:bold;
        }

        @media  only screen and (max-width: 600px) {
            .invoice-box table tr.top table td{
                width:100%;
                display:block;
                text-align:center;
            }

            .invoice-box table tr.information table td{
                width:100%;
                display:block;
                text-align:center;
            }

        }
    </style>
</head>

<body>
<div class='invoice-box'>
<!--    -->    <table cellpadding='0' cellspacing='0'>
        <tr>
            <td>
                Invoice #: " + _data.ReceiptNumber + @"<br>
                Created: " + _data.PaidOn + @"
            </td>
            <td colspan='2' style='text-align: center;'>

            </td>
            <td style='text-align: right;'>
                <strong> " + _data.BranchName + @"<br> </strong>
                " + _data.BranchAddress + @"<br>
                " + _data.BranchMobileNumber + @"
            </td>
        </tr>
    </table>
    <br>
    <table cellpadding='0' cellspacing='0'>
        <tr class='heading'>
            <td>Name</td>
            <td>Email</td>
            <td>Mobile</td>
            <td>Session(Class)</td>
        </tr>

        <tr class='details'>
                <td>" + _data.StudentName + @"</td>
                <td>" + _data.StudentEmail + @"</td>
                <td>" + _data.StudentMobileNumber + @"</td>
                <td>" + _data.SessionName + @"</td>
            </tr>
        </table>

        <table>
            <tr class='heading'>
                <td>Months</td>
                <td>Fine</td>
                <td>Discount</td>
                <td>Receipt Amount</td>
                <td>Paid Amount</td>
                <td>Pending</td>
                <td>Paid On</td>
            </tr>
            <tr class='item'>
                <td>" + _data.MonthsName + @"</td>
                <td>" + _data.TotalFine + @"</td>
                <td>" + _data.TotalDiscount + @"</td>
                <td>" + _data.TotalReceiptAmount + @"</td>
                <td>" + _data.TotalPaid + @"</td>
                <td>" + _data.PendingAmount + @"</td>
                <td>" + _data.PaidOn + @"</td>
            </tr>
        </table>
    <table cellpadding='0' cellspacing='0'>
        <tr style='border-top:1px solid #dddddd;'>
            <td style='text-align: right;font-size: 16px;'>
                <p>
                    <br>
                    <b>Signature</b>
                </p>
            </td>
        </tr>
    </table>
    <table cellpadding='0' cellspacing='0'>
        <tr style='border-top:1px solid #dddddd;'>
            <td style='text-align: center;font-size: 15px;'>
                <p>Fee is non refundable and non transferable in any condition</p> 
                <p>
                    Powered by <a href='www.protolabzit.com'>Protolabz E-Services</a>
                </p>
            </td>
        </tr>
    </table>
</div>
</body>
</html>";

            return print_data;
        }

        public string Get_InvoiceWithIdentityCard_PrintableData(PayFeeListData_VM _data, string Custom_Student_ID)
        {
            if (_data.BranchName == "")
            {
                _data.BranchName = First_Word_AppName + " " + Second_Word_AppName;
            }
            if (_data.BranchAddress == "")
            {
                _data.BranchAddress = "Mohali";
            }
            if (_data.BranchMobileNumber == "")
            {
                _data.BranchMobileNumber = "+91234567890";
            }
            string Student_Guardian = _data.Student_FatherName != "" ? _data.Student_FatherName : _data.Student_MotherName;

            string print_data = @"<!doctype html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Invoice</title>
    <style>
        .wrap_etc:first-child {
    margin-right: 10px;
}
table.second tr td {
    line-height: 1.77em;
}
        table.second tr td input {
            width: 125px;
        }
        .info h3 {
            text-align: center;
        }
        .invoice-box{
            max-width:1140px;
            margin:auto;
            padding:10px 30px;
            border:1px solid #eee;
            box-shadow:0 0 10px rgba(0, 0, 0, .15);
            font-size:18px;
            line-height:24px;
            font-family:'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
            color:#555;
        }

        .invoice-box table{
            width:100%;
            /* line-height:30px; */
            text-align:left;
        }

        .invoice-box table td{
            /* padding:5px; */
            vertical-align:top;
        }

        .invoice-box table tr td:nth-child(2){
            /*text-align:right;*/
        }

        .invoice-box table tr.top table td{
            padding-bottom:20px;
        }

        .invoice-box table tr.top table td.title{
            font-size:45px;
            line-height:45px;
            color:#333;
        }

        .invoice-box table tr.information table td{
            padding-bottom:40px;
        }

        .invoice-box table tr.heading td{
            background:#eee;
            border-bottom:1px solid #ddd;
            font-weight:bold;
        }

        .invoice-box table tr.details td{
            padding-bottom:20px;
        }

        .invoice-box table tr.item td{
            border-bottom:1px solid #eee;
        }

        .invoice-box table tr.item.last td{
            border-bottom:none;
        }

        .invoice-box table tr.total td:nth-child(2){
            border-top:2px solid #eee;
            font-weight:bold;
        }

        @media only screen and (max-width: 600px) {
            .invoice-box table tr.top table td{
                width:100%;
                display:block;
                text-align:center;
            }

            .invoice-box table tr.information table td{
                width:100%;
                display:block;
                text-align:center;
            }

        }

        /* front page css here */

    input.joing {
    border-bottom: 1px dotted #000 !important;
    border: 0px;
    /* width: 93% !important; */

  
    
}


        /* end front page css here */
  
        img.photo_left {
    height: 250px;
    width: 250px;
}
div#dvContents {
    margin-bottom: 30px !important;
}

.wrap_etc {
    border: 2px solid #eee;
}

@media print{

    body { 
    -webkit-print-color-adjust: exact; 
  }
.invoice-box table tr.heading td {
    background: #eee !important;
    border-bottom: 1px solid #ddd !important;
    font-weight: bold;
}
}

    </style>
</head>

<body>
<div class='invoice-box' style='margin-top:200px;'>

        <table cellpadding='0' cellspacing='0'>
            <tr>
                <td>
                    Invoice #: " + _data.ReceiptNumber + @"<br>
                    Created: " + _data.PaidOn + @"
                </td>
                <td colspan='2' style='text-align: center;'>

                </td>
                <td style='text-align: right;'>
                    <strong> " + _data.BranchName + @"<br> </strong>
                    " + _data.BranchAddress + @"<br>
                    " + _data.BranchMobileNumber + @"
                </td>
            </tr>
        </table>
    <br>
        <table cellpadding='0' cellspacing='0'>
            <tr class='heading'>
                <td>Name</td>
                <td>Email</td>
                <td>Mobile</td>
            </tr>

            <tr class='details'>
                <td>" + _data.StudentName + @"</td>
                <td>" + _data.StudentEmail + @"</td>
                <td>" + _data.StudentMobileNumber + @"</td>
            </tr>
        </table>

        <table>
            <tr class='heading'>
                <td>Session(Class)</td>
                <td>Fee</td>
                <td>Pending</td>
                <td>Discount</td>
                <td>Paid On</td>
            </tr>
            <tr class='item'>
                <td>" + _data.SessionName + @"</td>
                <td>" + _data.TotalPaid + @"</td>
                <td>" + _data.PendingAmount + @"</td>
                <td>" + _data.TotalDiscount + @"</td>
                <td>" + _data.PaidOn + @"</td>
            </tr>
        </table>

            <table cellpadding='0' cellspacing='0'>
                <tr style='border-top:1px solid #dddddd;'>
                    <td style='text-align: right;font-size: 16px;'>
                        <p>
                            <br>
                            <b>Signature</b>
                        </p>
                    </td>
                </tr>
            </table>


    <table cellpadding='0' cellspacing='0'>
        <tr style='border-top:1px solid #dddddd;'>
            <td style='text-align: center;font-size: 16px;'>
                <p>Fee is non refundable and non transferable in any condition</p> 
                <p>
                    Powered by <a href='www.protolabzit.com'>Protolabz E-Services</a>
                </p>
            </td>
        </tr>
    </table>
</div>
<div class='container' style='width: 1140px; margin: 0px auto; margin-top:430px !important;'>
    <div class='wrapper wrap_etc' id='dvContents' style='width: 46%;float: left;' >
        <!-- first div start here -->
    
    <table>
    <tr>
    <td class='left_img' style='width: 0%;'>
    <div class='left' style='float: left;'>
    <img src='"" + AppLogo + @""' alt='user' class='photo_left'>
    
    </div>
    </td>
    
       <!-- end left div -->
    
       <!-- start second right div -->

       <td class='text_rights'>
        <div class='right'>
            <div class='info'>
                <h3 style='font-size:22px;'>"" + First_Word_AppName + @""<p style='margin-top: 8px;'>"" + Second_Word_AppName + @""</p></h3>
                <div class='info_data'>
    
                <div class='data'>
    
                   <table>
                    <tbody>
                            <tr>
                                <td class='field_wrap' style='font-size: 18px;font-weight:bold;'>ID</td>
                                <td><span>:</span> <input type='text' style='font-size: 15px;width: 93%;' class='joing' value='"" + Custom_Student_ID + @""' readonly></td>
                            </tr>
                             <tr>
                                <td class='field_wrap' style='font-size: 18px;font-weight:bold;'>Name</td>
                                <td><span>:</span> <input type='text' style='font-size: 15px;width: 93%;' class='joing' value='"" + _data.StudentName + @""' readonly></td>
                            </tr>
                            <tr>
                                <td class='field_wrap' style='font-size: 18px;font-weight:bold;'>Guardian</td>
                                <td><span>:</span> <input type='text' style='font-size: 15px;width: 93%;' class='joing' value='"" + Student_Guardian + @""' readonly></td>
                            </tr>
    
                            <tr>
                              <td class='field_wrap' style='font-size: 18px;font-weight:bold;'>Session(Class)</td>
                              <td><span>:</span> <input type='text' style='font-size: 15px;width: 93%;' class='joing' value='"" + _data.SessionName + @""' readonly></td>
                          </tr>
    
                          <tr>
                            <td class='field_wrap' style='font-size: 18px;font-weight:bold;'>Contact</td>
                            <td><span>:</span> <input type='text' style='font-size: 15px;width: 93%;' class='joing' value='"" + _data.StudentMobileNumber + @""' readonly></td>
                        </tr>
    
                      <tr>
                      <td class='field_wrap' style='font-size: 18px;font-weight:bold;'>Regd. by</td>
                      <td><span>:</span> <input type='text' style='font-size: 15px;width: 93%;' class='joing' value='"" + _data.RegisteredBy + @""' readonly></td>
                      </tr>
                     </tbody>
                    </table>
                     </div>
                </div>
            </div>
        </div>
        </td>
        </tr>
    </table>
        
    </div> <!-- end front page design here -->

    <!-- start back page design here -->
    <div class='wrap_etc' style='float: left;width: 46%;'>
        <table class='second' style='margin-top:19px;margin-bottom:25px;width:95%;'>
            <tbody>
               
                <tr>
                    <td style='width:24%;padding-left:15px;font-size: 18px;font-weight:bold;'>Paid On</td>
                    <td style='width:76%;'><input type='text' style='width:100%;font-size: 15px;' class='joing' value='"" + _data.PaidOn + @""' readonly /></td>
                 
                </tr>
                <tr>
                    <td style='width:24%;padding-left:15px;font-size: 18px;font-weight:bold;'>Pending Fee</td>
                    <td style='width:76%;'> <input type='text' style='width:100%;font-size: 15px;' class='joing' value='"" + _data.PendingFees + @""' readonly></td>
                 
                </tr>

                 <tr>
                    <td style='width:24%;padding-left:15px;font-size: 18px;font-weight:bold;'>Remarks</td>
                    <td style='width:76%;'> <input type='text' style='width:100%;font-size: 15px;' class='joing' value='"" + _data.Remarks + @""' readonly></td>
                 
                </tr>

                <tr>
                    <td style='width:24%;padding-left:15px;font-size: 18px;font-weight:bold;'>Signature</td>
                    <td style='width:76%;'> <input type='text' style='width:100%;font-size: 15px;' class='joing' readonly></td>
                 
                </tr>
        
            </tbody>
        </table>
        </div>
        <div class='clear'>

        </div>
        <br />
    </div>
</body>
</html>";

            return print_data;
        }
    }
}