﻿var script = document.createElement('script');
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

window.onload = function () {    
    refreshCart();
}

function refreshCart() {
    var strMsg = "";
    if (localStorage.getItem("CartDetails") != undefined && localStorage.getItem("CartDetails") != '') {
        if (JSON.parse(localStorage.getItem("CartDetails")) != null && JSON.parse(localStorage.getItem("CartDetails")) != []) {
            CartDetails = JSON.parse(localStorage.getItem("CartDetails"));
            TotalAmount = 0;
            TotalCartItems = 0;
            CartDetails.forEach(function (item) {
                strMsg += '<div class="cart-table" id="' + item.ProductID + '"><div class="cart_Image"><img src=' + $("#img_" + item.ProductID + " img").attr("src") + '></div><div><strong class="splitData">' + item.Name + '</strong><h6>Rs.' + item.Price + '</h6><p style="text-align:bottom">Qty:- ' + item.Quantity + '</p></div><div class="cart-details-action" ><a href="#" style="writing-mode:vertical-lr" onclick="removeItemFromCart(' + item.ProductID + ',' + item.Quantity + ',' + item.Price + ')"><i class="fas fa-trash"></i></a></div></div>';
                TotalAmount += item.Amount;
                TotalCartItems += item.Quantity;
                $(".add_Cart_" + item.ProductID).addClass("displayNone");
                $(".cart_Count_" + item.ProductID).removeClass("displayNone");
                $("#item_Counter_" + item.ProductID).val(item.Quantity);
            });

            $("#appendCartData").empty().append(strMsg);

            $("#totalCartValue").empty().append('Total Cart Value:- <strong>Rs.' + TotalAmount + '</strong>');
            if (TotalCartItems > 0)
                $("#txtCartCount").text("(" + TotalCartItems + ")");
        }        
    }
}

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

        reCalculateCart(pId, prevItemCount, pPrice, "removeAll");

        var cartObj = {
            ProductID: parseInt(pId),
            Quantity: currentCount,
            Amount: pPrice * currentCount,
            Price: pPrice,
            Name: pName
        };

        CartDetails.push(cartObj);
        appendString = '<div class="cart-table" id="' + pId + '"><div class="cart_Image"><img src=' + $("#img_" + pId + " img").attr("src") + '></div><div><strong class="splitData">' + pName + '</strong><h6>Rs.' + pPrice + '</h6><p style="text-align:bottom">Qty:- ' + currentCount + '</p></div><div class="cart-details-action" ><a href="#" style="writing-mode:vertical-lr" onclick="removeItemFromCart(' + pId + ',' + currentCount + ',' + pPrice + ')"><i class="fas fa-trash"></i></a></div></div>';
        TotalAmount += pPrice * currentCount;
        TotalCartItems += currentCount;

        $(".add_Cart_" + pId).addClass("displayNone");
        $(".cart_Count_" + pId).removeClass("displayNone");
        $("#item_Counter_" + pId).val(currentCount);
    }
    else {
        $(".add_Cart_" + pId).addClass("displayNone");
        $(".cart_Count_" + pId).removeClass("displayNone");
        $("#item_Counter_" + pId).val(pQuantity);

        var cartObj = {
            ProductID: parseInt(pId),
            Quantity: pQuantity,
            Amount: pPrice * pQuantity,
            Price: pPrice,
            Name: pName
        };
        CartDetails.push(cartObj);
        appendString = '<div class="cart-table" id="' + pId + '"><div class="cart_Image"><img src=' + $("#img_" + pId + " img").attr("src") + '></div><div><strong class="splitData">' + pName + '</strong><h6>Rs.' + pPrice + '</h6><p style="text-align:bottom">Qty:- ' + pQuantity + '</p></div><div class="cart-details-action" ><a href="#" style="writing-mode:vertical-lr" onclick="removeItemFromCart(' + pId + ',' + pQuantity + ',' + pPrice + ')"><i class="fas fa-trash"></i></a></div></div>';
        TotalAmount += pPrice * pQuantity;
        TotalCartItems += pQuantity;
    }

    createToast("success", "Item Added to Cart..!!");

    $("#appendCartData").append(appendString);
    $("#totalCartValue").empty().append('Total Cart Value:- <strong>Rs.' + TotalAmount + '</strong>');
    $("#txtCartCount").text("(" + TotalCartItems + ")");

    //Set the localStorage
    localStorage.setItem("CartDetails", JSON.stringify(CartDetails));
}

function reCalculateCart(pId, pQuantity, pPrice, fromWhere) {
    TotalCartItems -= pQuantity;
    TotalAmount -= pPrice * pQuantity;
    $("#totalCartValue").empty().append('Total Cart Value:- <strong>Rs.' + TotalAmount + '</strong>');
    if (fromWhere == "removeAll")
        $("#" + pId).remove();    
}

function removeItem(pId, pQuantity, pPrice) {

    var CurrentItem = CartDetails.find(a => a.ProductID == pId);
    if (CurrentItem != null) {
        if (CurrentItem.Quantity > 1) {
            CurrentItem.Quantity -= pQuantity;
            CurrentItem.Amount = CurrentItem.Quantity * pPrice;
            $(".add_Cart_" + pId).addClass("displayNone");
            $(".cart_Count_" + pId).removeClass("displayNone");
            $("#item_Counter_" + pId).val(CurrentItem.Quantity);
            reCalculateCart(pId, pQuantity, pPrice, "removeOneItem");
            $("#txtCartCount").text("(" + TotalCartItems + ")");
        }
        else {
            $(".add_Cart_" + pId).removeClass("displayNone");
            $(".cart_Count_" + pId).addClass("displayNone");
            $("#item_Counter_" + pId).val(0);
            reCalculateCart(pId, pQuantity, pPrice, "removeAll");
            if (TotalCartItems < 1)
                $("#txtCartCount").text("");
            else
                $("#txtCartCount").text("(" + TotalCartItems + ")");
            CartDetails = CartDetails.filter(function (item) {
                return item.ProductID !== parseInt(pId)
            });
        }
        //Update the localStorage
        localStorage.setItem("CartDetails", JSON.stringify(CartDetails));
        refreshCart();

        createToast("info", "Item Removed From Cart..!!");
    }
}

function removeItemFromCart(pId, pQuantity, pPrice) {

    CartDetails = CartDetails.filter(function (item) {
        return item.ProductID !== parseInt(pId)
    });

    TotalCartItems -= pQuantity;
    TotalAmount -= pPrice * pQuantity;
    $("#totalCartValue").empty().append('Total Cart Value:- <strong>Rs.' + TotalAmount + '</strong>');
    $("#" + pId).remove();

    //Update the localStorage
    localStorage.setItem("CartDetails", JSON.stringify(CartDetails));
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

function ProceedToPayment(e) {
    $("#proceedToPaymentBtn").attr("disabled", true);
    e.preventDefault();
    var form = $('#paymentPageForm');
    var actionUrl = form.attr('action');
    var method = form.attr('method');

    $.ajax({
        type: method,
        url: actionUrl,
        data: form.serialize(),
        success: function (data) {
            if (data.IsSuccess) {
                CartDetails = [];
                localStorage.setItem("CartDetails", CartDetails);
                createToast("success", data.Message);
                setTimeout(function () { window.location.href = "../Products/BuyerOrderDetails?from=Payment&orderID=" + $('#txtOrderID').val();}, 3000);
            }
            else {
                $("#proceedToPaymentBtn").attr("disabled", true);
                createToast("error", data.Message);
            }
        }
    });
};

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
    var strMsg="";
    if (CartDetails == [] || CartDetails == "[]" || CartDetails == null || CartDetails == undefined || CartDetails.length <= 0) {
        var colorTheme, popupHeader, btnText;
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

function getImagePreview(event) {
    var img = URL.createObjectURL(event.target.files[0]);

    $("#imgPreview").removeClass("hidden").empty().append("<img class='product_Img' src='" + img + "' />");
}

function AddNewScreen(id) {
    if (parseInt($(".addTheatre-card").length) - 1 >= parseInt($("#NoOfScreens")[0].innerText)) {
        alert("No of Screens Cannot Exceed the Theatre's Screen Limit.");
        return 0;
    }
    $.ajax({
        type: 'GET',
        url: ("../AddNewScreen/" + id),
        contentType: "application/json",
        success: function (result) {
            $("body").html(result);
        }
    });
}

// Object containing details for different types of toasts
const toastDetails = {
    timer: 5000,
    success: {
        icon: 'fa-circle-check',
        text: 'Success: This is a success toast.',
    },
    error: {
        icon: 'fa-circle-xmark',
        text: 'Error: This is an error toast.',
    },
    info: {
        icon: 'fa-circle-info',
        text: 'Info: This is an information toast.',
    }
}

const removeToast = (toast) => {
    toast.classList.add("hide");
    if (toast.timeoutId) clearTimeout(toast.timeoutId); // Clearing the timeout for the toast
    setTimeout(() => toast.remove(), 500); // Removing the toast after 500ms
}

function createToast(id, text){

    const notifications = document.querySelector(".notifications");

    // Getting the icon and text for the toast based on the id passed
    const { icon } = toastDetails[id];
    const toast = document.createElement("li"); // Creating a new 'li' element for the toast
    toast.className = `toast ${id}`; // Setting the classes for the toast
    // Setting the inner HTML for the toast
    toast.innerHTML = `<div class="column">
                        <i class="fa-solid ${icon}"></i>
                        <span>${text}</span>
                        </div>
                        <i class="fa-solid fa-xmark" onclick="removeToast(this.parentElement)"></i>`;

    notifications.appendChild(toast); // Append the toast to the notification ul
    // Setting a timeout to remove the toast after the specified duration
    toast.timeoutId = setTimeout(() => removeToast(toast), toastDetails.timer);
}

function SubmitSellerOrderUpdate(e) {
    e.preventDefault();
    var form = $("#submitSellerOrderUpdate");
    var actionUrl = form.attr('action');
    var method = form.attr('method');

    $.ajax({
        type: method,
        url: actionUrl,
        data: form.serialize(),
        success: function (data) {
            if (data.IsSuccess) {
                createToast("success", data.Message);
            }
            else {
                createToast("error", data.Message);
            }
        }
    });
};

function PrintOrderSummary(e){
    e.preventDefault();
    window.print();
}

function ViewProductDetails(productID) {    
    $.ajax({
        type: "GET",
        url: "../Products/ViewProductDetails/" + parseInt(productID),
        success: function (data) {
            $("#viewProductModal").empty();
            $("#viewProductModal").html(data);
            $("#viewProductModal").removeAttr("style");
        }
    });
}

function DeleteProduct(id) {
    var con = confirm("Are you Sure? Do you want to delete this product?");
    if (con) {
        $.ajax({
            type: "GET",
            url: "../Products/DeleteProduct/" + parseInt(id),
            success: function (data) {
                if (data.IsSuccess) {
                    createToast("success", data.Message);
                    setTimeout(function () { window.location.reload() }, 5000);
                }
                else {
                    createToast("error", data.Message);
                }
            }
        });
    }
};


function CloseProdModal() {
    var prodModal = document.getElementById('viewProductModal');
    $("#viewProductModal").empty();
    prodModal.close();
    $("#viewProductModal").attr("style", "display: none;");
};

function UpdateUserStatus(userName) {
    if (confirm("Do you want to change the status?")) {
        $.ajax({
            type: "POST",
            url: "/Account/UpdateActiveStatus?userName=" + userName,
            success: function (data) {
                if (data.IsSuccess) {
                    if ($("#" + userName).hasClass("btn-success")) {
                        $("#" + userName).removeClass("btn-success");
                        $("#" + userName).addClass("btn-primary");
                        $("#" + userName).html('<i class="fas fa-edit"></i> In Active')
                        createToast("info", "User's is now InActive.");
                    }
                    else if ($("#" + userName).hasClass("btn-primary")) {
                        $("#" + userName).removeClass("btn-primary");
                        $("#" + userName).addClass("btn-success");
                        $("#" + userName).html('<i class="fas fa-edit"></i> Active')
                        createToast("success", "User's is now Active.");
                    }
                }
                else {
                    createToast("error", data.Message);
                }
            }
        });
    }
    else {
        createToast("error", "Request Cancelled.");
    }
};
