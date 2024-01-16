var script = document.createElement('script');
script.src = '/Scripts/jquery-3.4.1.js';
document.getElementsByTagName('head')[0].appendChild(script);  

var PrevBtnValue = 0;
var IsAuthenticated = false;
var TotalAmount = 0;
var TotalCartItems = 0;
var CartDetails = [];
var OrderID;
var selectedSeatNo = "";
var MovieDetails = [];
var Data;

function OnChangePaymentMode(obj) {
    var PaymentMode = $(obj).val();

    if (PaymentMode != null && PaymentMode != "") {
        switch (PaymentMode) {
            case "Online":
                $("#divUPIPayment").fadeOut("slow");
                $("#divChequePayment").fadeOut("slow");
                break;
            case "Cheque":
                $("#divChequePayment").fadeIn("slow");
                $("#divUPIPayment").fadeOut("slow");
                $("#divUPIPayment").hide();
                break;
            case "UPI":
                $("#divUPIPayment").fadeIn("slow");
                $("#divChequePayment").fadeOut("slow");
                $("#divChequePayment").hide();
                break;
        }
    }
}

function AddtoCart(obj, pName, pId, pQuantity, pPrice) {

    if (TotalCartItems >= 10) {
        var strMsg, colorTheme, popupHeader, btnText;
        strMsg = "Cannot Add More Than 10 Items To The Cart..!! ";
        colorTheme = "fail";
        popupHeader = "Alert";
        btnText = "Close";
        openSuccessModal(strMsg, colorTheme, popupHeader, btnText, "");
        return false;
    }

    if (!$("#btnProceed").is(":visible")) {
        $("#btnProceed").parent().removeClass("displayNone");
    }

    var prevItemCount = 0;
    var currentCount = 0;
    var appendString = "";

    console.log($("#img_" + pId + " img").attr("src"));

    if (CartDetails != [] && CartDetails != "[]" && CartDetails != null && CartDetails != undefined && CartDetails.length > 0) {
        CartDetails.forEach(function (item, index, arr) {
            if (item.ProductID == pId) {
                prevItemCount = item.Quantity;
            }
        });        

        currentCount = parseInt(prevItemCount) + pQuantity;

        if (currentCount >= (parseInt($(obj).attr("data-attr-MaxQuantity")) + pQuantity)) {
            var strMsg, colorTheme, popupHeader, btnText;
            strMsg = "Max Limit reached for " + pName;
            colorTheme = "fail";
            popupHeader = "Alert";
            btnText = "Close";
            openSuccessModal(strMsg, colorTheme, popupHeader, btnText, "");
            return false;
        }

        CartDetails = CartDetails.filter(function (item) {
            return item.ProductID !== parseInt(pId)
        });

        reCalculateCart(pId, prevItemCount, pPrice);

        var cartObj = {
            ProductID: parseInt(pId),
            Quantity: currentCount,
            Amount: pPrice * currentCount
        };

        CartDetails.push(cartObj);
        appendString = '<div class="cart-table" id="' + pId + '"><div class="cart-product-image"><img src=' + $("#img_" + pId + " img").attr("src") + '/></div><div><strong class="splitData">' + pName + '</strong><h6>Rs.' + pPrice + '</h6><p style="text-align:bottom">Qty:- ' + currentCount + '</p></div><div class="cart-details-action" style="margin: auto; width: 50 %;" ><a href="#" style="writing-mode:vertical-lr" onclick="removeItemFromCart(' + pId + ',' + currentCount + ',' + pPrice + ')"><i class="fas fa-trash"></i></a></div></div>';
        TotalAmount += pPrice * currentCount;
        TotalCartItems += currentCount;
    }
    else {
        var cartObj = {
            ProductID: parseInt(pId),
            Quantity: pQuantity,
            Amount: pPrice * pQuantity
        };
        CartDetails.push(cartObj);
        appendString = '<div class="cart-table" id="' + pId + '"><div class="cart-product-image"><img src=' + $("#img_" + pId + " img").attr("src") + '/></div><div><strong class="splitData">' + pName + '</strong><h6>Rs.' + pPrice + '</h6><p style="text-align:bottom">Qty:- ' + pQuantity + '</p></div><div class="cart-details-action" style="margin: auto; width: 50 %;" ><a href="#" style="writing-mode:vertical-lr" onclick="removeItemFromCart(' + pId + ',' + pQuantity + ',' + pPrice + ')"><i class="fas fa-trash"></i></a></div></div>';
        TotalAmount += pPrice * pQuantity;
        TotalCartItems += pQuantity;
    }

    $("#appendCartData").append(appendString);
    $("#totalCartValue").empty().append('Total Cart Value:- <strong>Rs.' + TotalAmount + '</strong>');
    $("#txtCartCount").text("(" + TotalCartItems + ")");

}

function reCalculateCart(pId, pQuantity, pPrice) {
    TotalCartItems -= pQuantity;
    TotalAmount -= pPrice * pQuantity;
    $("#totalCartValue").empty().append('Total Cart Value:- <strong>Rs.' + TotalAmount + '</strong>');
    $("#" + pId).remove();
}

function removeItemFromCart(pId, pQuantity, pPrice) {

    CartDetails = CartDetails.filter(function (item) {
        return item.ProductID !== parseInt(pId)
    });

    TotalCartItems -= pQuantity;
    TotalAmount -= pPrice * pQuantity;
    $("#totalCartValue").empty().append('Total Cart Value:- <strong>Rs.' + TotalAmount + '</strong>');
    $("#" + pId).remove();
}

function ProceedToCheckout(PaymentOf) {

    if (PaymentOf == "ECommerce") {
        if (!toBool(IsAuthenticated)) {

            var strMsg, colorTheme, popupHeader, btnText;
            strMsg = "Please Login to Purchase Items..!!";
            colorTheme = "fail";
            popupHeader = "Alert";
            btnText = "Login";
            openSuccessModal(strMsg, colorTheme, popupHeader, btnText, "");

            return false;
        }

        if (CartDetails == [] || CartDetails == "[]" || CartDetails == null || CartDetails == undefined || CartDetails.length <= 0) {
            var strMsg, colorTheme, popupHeader, btnText;
            strMsg = "Please Add Items To The Cart..!! ";
            colorTheme = "fail";
            popupHeader = "Alert";
            btnText = "Close";
            openSuccessModal(strMsg, colorTheme, popupHeader, btnText, "");
            return false;
        }

        TotalAmount = 0;
        TotalCartItems = 0;

        Data = JSON.stringify(CartDetails);
    }
    else if (PaymentOf == "MovieTickets")
    {
        MovieDetails = {
            "SeatNo": $("#SeatNo").val(),
            "ScreenTimingConfigID_FK": $("#hdnScreenTimingConfigID_FK").val(),
            "NoOfSeats": $("#txtNoOfSeats").val(),
            "SeatCategory": $("#ddlSeatCategory").val()
        };

        Data = JSON.stringify(MovieDetails);
    }

    $.ajax({
        type: 'POST',
        url: "../Payment/Payment?PaymentOf=" + PaymentOf,
        data: Data,
        contentType: "application/json",
        success: function (result) {
            $("body").html(result);
        }
    });
    
}

function toBool(data) {
    if (data.toLowerCase() == "true") {
        return true
    }
    else
        return false;
}

function openSuccessModal(strMsg, colorTheme, popupHeader, btnText, value ) {

    if (colorTheme == "fail") {
        $(".modalHeaderCommon, .modalfooterCommon").attr("style", "background-color:tomato;");
    }

    if (btnText == "Login") {
        $(".closeModal").on("click", function () {
            window.location.href = "Account/Login";
        });
    }
    else if (btnText == "OrderDetails") {
        $(".closeModal").on("click", function () {
            window.location.href = "../Products/BuyerOrderDetails?orderID=" + value;
        });
    }

    if (colorTheme == "AddressSuccess") {
        $(".closeModal").on("click", function () {

            var modal = document.getElementById("commonPopUp");
            var span = document.getElementsByClassName("close")[0];
            span.onclick = function () {
                $("#commonPopUp").attr("style", "display:none");
            }
            var btn = document.getElementsByClassName("closeModal")[0];
            btn.onclick = function () {
                $("#commonPopUp").attr("style", "display:none");
            }

            window.onclick = function (event) {
                if (event.target == modal) {
                    $("#commonPopUp").attr("style", "display:none");
                }
            }

            GetAddress(value);
        });
    }

    $(".closeModal").text(btnText);
    $("#popupHeader").text(popupHeader);
    $("#popupBodyMsg").text(strMsg);
    $("#commonPopUp").attr("style", "display:block");
}

function AddReview(OrderID, pID, pName) {
    $.ajax({
        type: 'GET',
        url: ("../Products/AddReview?orderID=" + OrderID + "&pID=" + parseInt(pID) + "&pName=" + pName),
        contentType: "application/json",
        success: function (result) {
            $("#revPopupBodyMsg").html(result);

            $("#addReviewPopUp").attr("style", "display:block");
        }
    });
}

function GetCity(obj) {
    var stateID = $(obj).val();
    $.ajax({
        type: 'GET',
        url: ("../Account/GetCityList/?stateID=" + stateID),
        contentType: "application/json",
        success: function (data) {
            $("#ddlCity").empty();
            $("#ddlCity").append("<option val=0> Please Select City</option>");
            data.forEach(function (item, index, arr) {
                $("#ddlCity").append("<option val=" + parseInt(item.Value) + "> " + item.Text + "</option>");
            });
        }
    });
}

function GetPincode(obj) {
}      

function AddAddress(type) {
    $.ajax({
        type: 'GET',
        url: ("../Account/AddAddress?type=" + type),
        contentType: "application/json",
        success: function (result) {
            $("#addressPopupBodyMsg").html(result);
            $("#addAddressPopUp").attr("style", "display:block");
        }
    });
}

function GetAddress(type) {
    $.ajax({
        type: 'GET',
        url: ("../Payment/GetJsonAddressByUserID"),
        contentType: "application/json",
        success: function (data) {
            $("#billingAddress").empty();
            $("#shippingAddress").empty();
            var str = "";
            data.forEach(function (item, index, arr) {
                if (item.Type == "Billing") {
                    str = '<div class="addressCart" ><div><input name="BillingAddressID" type="radio" value="' + item.AddressID + '" required="required"/></div><div style="border-radius:10px; width:400px "><div style=" display: flex; flex-direction: row; justify-content: space-between; align-items:center "><h5>' + item.Name + '</h4><h6>' + item.City + '-' + item.State + '</h6></div><div style=" display: flex; flex-direction: row; justify-content: space-between; "><h6>'+ item.FullAddress + '</h6></div></div></div >';
                    $("#billingAddress").append(str);
                }
                else {
                    str = '<div class="addressCart" style = "border-color:red" ><div><input name="ShippingAddressID" type="radio" value="' + item.AddressID + '" required="required"/></div><div style="border-radius:10px; width:400px "><div style=" display: flex; flex-direction: row; justify-content: space-between; align-items:center "><h5>' + item.Name + '</h4><h6>' + item.City + ' - ' + item.State + '</h6></div><div style=" display: flex; flex-direction: row; justify-content: space-between; "><h6>' + item.FullAddress + '</h6></div></div></div >';
                    $("#shippingAddress").append(str);
                }
            });
        }
    });        
}

$("#paymentPageForm").submit(function (e) {

    e.preventDefault();

    var form = $(this);
    var actionUrl = form.attr('action');
    var method = form.attr('method');

    $.ajax({
        type: method,
        url: actionUrl,
        data: form.serialize(),
        success: function (data) {
            if (data.IsSuccess) {
                var strMsg, colorTheme, popupHeader, btnText;
                strMsg = "Your Order Has Been Placed Successfully..!!";
                colorTheme = "success";
                popupHeader = "Alert";
                btnText = "OrderDetails";
                openSuccessModal(strMsg, colorTheme, popupHeader, btnText, data.OrderID);
            }
            else {
                var strMsg, colorTheme, popupHeader, btnText;
                strMsg = data.ErrorMsg;
                colorTheme = "fail";
                popupHeader = "Alert";
                btnText = "Close";
                openSuccessModal(strMsg, colorTheme, popupHeader, btnText, "");
            }
        }
    });
});

function SubmitAddress() {
    var form = $("#addAddressForm");
    var actionUrl = form.attr('action');
    var method = form.attr('method');

    $.ajax({
        type: method,
        url: actionUrl,
        data: form.serialize(),
        success: function (data) {
            if (data.IsSuccess) {
                var strMsg, colorTheme, popupHeader, btnText;
                strMsg = "Address Added Successfully..!!";
                colorTheme = "AddressSuccess";
                popupHeader = "Alert";
                btnText = "Back";
                openSuccessModal(strMsg, colorTheme, popupHeader, btnText, data.Type);
            }
            else {
                var strMsg, colorTheme, popupHeader, btnText;
                strMsg = data.ErrorMsg;
                colorTheme = "fail";
                popupHeader = "Alert";
                btnText = "Close";
                openSuccessModal(strMsg, colorTheme, popupHeader, btnText, "");
            }
        }
    });
};

function OpenCart() {
    if (CartDetails == [] || CartDetails == "[]" || CartDetails == null || CartDetails == undefined || CartDetails.length <= 0) {
        var strMsg, colorTheme, popupHeader, btnText;
        strMsg = "Please Add Items To The Cart..!! ";
        colorTheme = "fail";
        popupHeader = "Alert";
        btnText = "Close";
        openSuccessModal(strMsg, colorTheme, popupHeader, btnText, "");
        return false;
    }
    $("#cartPopUp").attr("style", "display:flex;");
}

function OpenSelectSeats(obj, divToOpen) {
    var ScreenTimingConfigID = $(obj).attr("data-attr-value");

    if (PrevBtnValue != 0 && PrevBtnValue != ScreenTimingConfigID) {
        $(".seatCategorySelected").removeClass("seatCategorySelected");
        $(".divOpenSelectSeat").each(function (inx) {
            if ($(this).is(":visible")) {
                $(this).slideUp();
                $(this).empty();
            }
        })
    }

    PrevBtnValue = ScreenTimingConfigID;

    if ($(obj).hasClass("seatCategorySelected")) {
        $("#" + divToOpen).slideUp();
        $("#" + divToOpen).empty();
        $(obj).removeClass("seatCategorySelected");
    }
    else {
        $.ajax({
            type: 'GET',
            url: "../BookTicket/SelectSeats?ScreenTimingConfigID=" + ScreenTimingConfigID,
            success: function (data) {
                $("#" + divToOpen).empty();
                $("#" + divToOpen).html(data);
                $("#" + divToOpen).slideDown();
            }
        });
        $(obj).addClass("seatCategorySelected");
    }  
}

function OnChangeSeatCategory(obj) {
    var SeatCategory = $(obj).find("option:selected").text();
    $(".seatSelected").removeClass("seatSelected");
    $("#seatContainer").removeAttr("style");

    var dropData = $("#SeatCategory").text()
    dropData = dropData.split("\n");

    dropData.forEach(function (item, index, arr) {
        if (item == SeatCategory) {
            $("#divParent" + SeatCategory).removeAttr("style");
        }
        else {
            $("#divParent" + item).attr("style", "pointer-events:none; opacity:.5;");
        }
    });
    selectedSeatNo = "";
    $("#seatContainer").removeAttr("style");
}