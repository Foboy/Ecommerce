﻿/*
** nopCommerce ajax cart implementation
*/


var AjaxCart = {
    loadWaiting: false,
    usepopupnotifications: false,
    topcartselector: '',
    topwishlistselector: '',
    flyoutcartselector: '',

    init: function (usepopupnotifications, topcartselector, topwishlistselector, flyoutcartselector) {
        this.loadWaiting = false;
        this.usepopupnotifications = usepopupnotifications;
        this.topcartselector = topcartselector;
        this.topwishlistselector = topwishlistselector;
        this.flyoutcartselector = flyoutcartselector;
    },

    setLoadWaiting: function (display) {
        displayAjaxLoading(display);
        this.loadWaiting = display;
    },

    //add a product to the cart/wishlist from the catalog pages
    addproducttocart_catalog: function (urladd) {
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);
        var me = this;
        $.ajax({
            cache: false,
            url: urladd,
            type: 'post',
            success: function (response) {
                me.success_desktop(response);
            },
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    //add a product to the cart/wishlist from the product details page (desktop version)
    addproducttocart_details: function (urladd, formselector, checkoutUrl) {
        if (this.loadWaiting != false) {
            return;
        }
        var me = this;
        this.setLoadWaiting(true);
        $.ajax({
            cache: false,
            url: urladd,
            data: $(formselector).serialize(),
            type: 'post',
            success: function (response) {
                me.success_desktop(response, checkoutUrl);
            },
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },
    addproducttopackage_details: function (urladd, formselector, checkoutUrl) {
        if (this.loadWaiting != false) {
            return;
        }
        var me = this;
        this.setLoadWaiting(true);
        $.ajax({
            cache: false,
            url: urladd,
            data: $(formselector).serialize(),
            type: 'post',
            success: function (response) {
                if (response.success == true) {
                    alert(response.message);
                    window.close();
                }
                else {
                    alert(response.message)
                }
                me.success_desktop(response, checkoutUrl);
            },
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    //add a product to the cart/wishlist from the product details page (mobile devices version)
    addproducttocart_details_mobile: function (urladd, formselector, successredirecturl) {
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);

        $.ajax({
            cache: false,
            url: urladd,
            data: $(formselector).serialize(),
            type: 'post',
            success: function (response) {
                //if (response.updatetopcartsectionhtml) {
                //    $(AjaxCart.topcartselector).html(response.updatetopcartsectionhtml);
                //}
                //if (response.updatetopwishlistsectionhtml) {
                //    $(AjaxCart.topwishlistselector).html(response.updatetopwishlistsectionhtml);
                //}
                if (response.message) {
                    //display notification
                    if (response.success == true) {
                        //we do not display success message in mobile devices mode
                        //just redirect a user to the cart/wishlist
                        location.href = successredirecturl;
                    }
                    else {
                        //error
                        displayStandardAlertNotification(response.message);
                    }
                    return false;
                }
                if (response.redirect) {
                    location.href = response.redirect;
                    return true;
                }
                return false;
            },
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    success_desktop: function (response, checkoutUrl) {
        console.log(response)
        if (response.updatetopcartsectionhtml) {
            $(AjaxCart.topcartselector).html(response.updatetopcartsectionhtml);
        }
        if (response.updatetopwishlistsectionhtml) {
            $(AjaxCart.topwishlistselector).html(response.updatetopwishlistsectionhtml);
        }
        if (response.updateflyoutcartsectionhtml) {
            
            $(AjaxCart.flyoutcartselector).html($("<div>" + response.updateflyoutcartsectionhtml + "</div>").find(AjaxCart.flyoutcartselector).html());
        }
        if (response.message) {
            //display notification
            if (response.success == true) {

                if (typeof checkoutUrl != "undefined")
                {
                    location.href = checkoutUrl;
                    return true;
                }
                //success
                if (AjaxCart.usepopupnotifications == true) {
                    displayPopupNotification(response.message, 'success', true);
                }
                else {
                    //specify timeout for success messages
                    displayBarNotification(response.message, 'success', 3500);
                }
            }
            else {
                //error
                if (AjaxCart.usepopupnotifications == true) {
                    displayPopupNotification(response.message, 'error', true);
                }
                else {
                    //no timeout for errors
                    displayBarNotification(response.message, 'error', 3500);
                }
                
            }
            return false;
        }
        if (response.redirect) {
            location.href = response.redirect;
            return true;
        }
        return false;
    },

    resetLoadWaiting: function () {
        AjaxCart.setLoadWaiting(false);
    },

    ajaxFailure: function () {
        alert('添加失败');
    }
};